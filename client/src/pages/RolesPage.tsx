import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import type { Role } from '../types';
import * as api from '../services/api';

export default function RolesPage() {
  const queryClient = useQueryClient();
  const [showForm, setShowForm] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState({ name: '', hourlyRate: 0 });
  const [validationError, setValidationError] = useState('');
  const [apiError, setApiError] = useState('');

  const { data: roles, isLoading, error } = useQuery<Role[]>({
    queryKey: ['roles'],
    queryFn: api.getRoles,
  });

  const createMutation = useMutation({
    mutationFn: (role: Omit<Role, 'id'>) => api.createRole(role),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['roles'] });
      handleCancel();
      setApiError('');
    },
    onError: (error: Error) => {
      setApiError(error.message || 'Failed to create role');
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, role }: { id: number; role: Omit<Role, 'id'> }) =>
      api.updateRole(id, role),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['roles'] });
      handleCancel();
      setApiError('');
    },
    onError: (error: Error) => {
      setApiError(error.message || 'Failed to update role');
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.deleteRole(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['roles'] });
      setApiError('');
    },
    onError: (error: Error) => {
      setApiError(error.message || 'Failed to delete role');
    },
  });

  const validateForm = (): boolean => {
    setValidationError('');
    
    const trimmedName = formData.name.trim();
    if (!trimmedName) {
      setValidationError('Name is required');
      return false;
    }

    if (formData.hourlyRate < 0) {
      setValidationError('Hourly rate must be 0 or greater');
      return false;
    }

    return true;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    const roleData = {
      name: formData.name.trim(),
      hourlyRate: formData.hourlyRate,
    };

    if (editingId) {
      updateMutation.mutate({ id: editingId, role: roleData });
    } else {
      createMutation.mutate(roleData);
    }
  };

  const handleEdit = (role: Role) => {
    setFormData({ name: role.name, hourlyRate: role.hourlyRate });
    setEditingId(role.id);
    setShowForm(true);
    setValidationError('');
    setApiError('');
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Are you sure you want to delete this role?')) return;
    deleteMutation.mutate(id);
  };

  const handleCancel = () => {
    setFormData({ name: '', hourlyRate: 0 });
    setShowForm(false);
    setEditingId(null);
    setValidationError('');
    setApiError('');
  };

  const handleNewRole = () => {
    setShowForm(true);
    setValidationError('');
    setApiError('');
  };

  if (isLoading) {
    return <div className="p-8">Loading...</div>;
  }

  if (error) {
    return (
      <div className="p-8">
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
          Failed to load roles
        </div>
      </div>
    );
  }

  return (
    <div className="max-w-6xl mx-auto p-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Roles</h1>
        <button
          onClick={handleNewRole}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
        >
          New Role
        </button>
      </div>

      {apiError && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {apiError}
        </div>
      )}

      {showForm && (
        <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
          <h2 className="text-xl font-bold mb-4">
            {editingId ? 'Edit Role' : 'Add New Role'}
          </h2>
          <form onSubmit={handleSubmit}>
            <div className="mb-4">
              <label className="block text-gray-700 text-sm font-bold mb-2">
                Name
              </label>
              <input
                type="text"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              />
            </div>
            <div className="mb-4">
              <label className="block text-gray-700 text-sm font-bold mb-2">
                Hourly Rate
              </label>
              <input
                type="number"
                step="0.25"
                value={formData.hourlyRate}
                onChange={(e) => setFormData({ ...formData, hourlyRate: parseFloat(e.target.value) || 0 })}
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              />
            </div>
            {validationError && (
              <div className="mb-4 text-red-600 text-sm">
                {validationError}
              </div>
            )}
            <div className="flex items-center justify-between">
              <button
                type="submit"
                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                disabled={createMutation.isPending || updateMutation.isPending}
              >
                {createMutation.isPending || updateMutation.isPending
                  ? 'Saving...'
                  : editingId
                  ? 'Update'
                  : 'Create'}
              </button>
              <button
                type="button"
                onClick={handleCancel}
                className="bg-gray-500 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
              >
                Cancel
              </button>
            </div>
          </form>
        </div>
      )}

      <div className="bg-white shadow-md rounded overflow-hidden">
        <table className="min-w-full">
          <thead className="bg-gray-100">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Name
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Hourly Rate
              </th>
              <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                Actions
              </th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {roles?.map((role) => (
              <tr key={role.id}>
                <td className="px-6 py-4 whitespace-nowrap text-gray-900">
                  {role.name}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-gray-900">
                  ${role.hourlyRate.toFixed(2)}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  <button
                    onClick={() => handleEdit(role)}
                    className="text-blue-600 hover:text-blue-900 mr-4"
                  >
                    Edit
                  </button>
                  <button
                    onClick={() => handleDelete(role.id)}
                    className="text-red-600 hover:text-red-900"
                    disabled={deleteMutation.isPending}
                  >
                    {deleteMutation.isPending ? 'Deleting...' : 'Delete'}
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {roles?.length === 0 && (
          <div className="text-center py-8 text-gray-500">
            No roles found. Add one to get started!
          </div>
        )}
      </div>
    </div>
  );
}
