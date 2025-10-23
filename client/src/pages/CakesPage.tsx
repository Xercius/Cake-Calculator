import { useState, useEffect } from 'react';
import type { Cake, Template } from '../types';
import * as api from '../services/api';

export default function CakesPage() {
  const [cakes, setCakes] = useState<Cake[]>([]);
  const [templates, setTemplates] = useState<Template[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState({
    name: '',
    templateId: 0,
    extraIngredients: '{}',
    labor: 0,
    otherCosts: 0,
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [cakesData, templatesData] = await Promise.all([
        api.getCakes(),
        api.getTemplates(),
      ]);
      setCakes(cakesData);
      setTemplates(templatesData);
      setError('');
    } catch (err) {
      setError('Failed to load data');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      // Validate JSON
      if (formData.extraIngredients) {
        JSON.parse(formData.extraIngredients);
      }
      
      if (editingId) {
        await api.updateCake(editingId, formData);
      } else {
        await api.createCake(formData);
      }
      setFormData({ name: '', templateId: 0, extraIngredients: '{}', labor: 0, otherCosts: 0 });
      setShowForm(false);
      setEditingId(null);
      loadData();
    } catch (err) {
      setError('Failed to save cake. Check JSON format.');
    }
  };

  const handleEdit = (cake: Cake) => {
    setFormData({
      name: cake.name,
      templateId: cake.templateId,
      extraIngredients: cake.extraIngredients || '{}',
      labor: cake.labor,
      otherCosts: cake.otherCosts,
    });
    setEditingId(cake.id);
    setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Are you sure you want to delete this cake?')) return;
    try {
      await api.deleteCake(id);
      loadData();
    } catch (err) {
      setError('Failed to delete cake');
    }
  };

  const handleCancel = () => {
    setFormData({ name: '', templateId: 0, extraIngredients: '{}', labor: 0, otherCosts: 0 });
    setShowForm(false);
    setEditingId(null);
  };

  if (loading) return <div className="p-8">Loading...</div>;

  return (
    <div className="max-w-6xl mx-auto p-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Cakes</h1>
        <button
          onClick={() => setShowForm(true)}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
        >
          Add Cake
        </button>
      </div>

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}

      {showForm && (
        <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
          <h2 className="text-xl font-bold mb-4">
            {editingId ? 'Edit Cake' : 'Add New Cake'}
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
                required
              />
            </div>
            <div className="mb-4">
              <label className="block text-gray-700 text-sm font-bold mb-2">
                Template
              </label>
              <select
                value={formData.templateId}
                onChange={(e) => setFormData({ ...formData, templateId: parseInt(e.target.value) })}
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                required
              >
                <option value={0}>Select a template</option>
                {templates.map((template) => (
                  <option key={template.id} value={template.id}>
                    {template.name} - {template.size} - {template.type}
                  </option>
                ))}
              </select>
            </div>
            <div className="mb-4">
              <label className="block text-gray-700 text-sm font-bold mb-2">
                Extra Ingredients (JSON)
              </label>
              <textarea
                value={formData.extraIngredients}
                onChange={(e) => setFormData({ ...formData, extraIngredients: e.target.value })}
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline font-mono text-sm"
                rows={3}
                placeholder='{"1": 0.5}'
              />
              <p className="text-gray-600 text-xs mt-1">
                Format: {`{"ingredientId": quantity}`}
              </p>
            </div>
            <div className="mb-4">
              <label className="block text-gray-700 text-sm font-bold mb-2">
                Labor Cost
              </label>
              <input
                type="number"
                step="0.01"
                value={formData.labor}
                onChange={(e) => setFormData({ ...formData, labor: parseFloat(e.target.value) })}
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                required
              />
            </div>
            <div className="mb-4">
              <label className="block text-gray-700 text-sm font-bold mb-2">
                Other Costs
              </label>
              <input
                type="number"
                step="0.01"
                value={formData.otherCosts}
                onChange={(e) => setFormData({ ...formData, otherCosts: parseFloat(e.target.value) })}
                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                required
              />
            </div>
            <div className="flex items-center justify-between">
              <button
                type="submit"
                className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
              >
                {editingId ? 'Update' : 'Create'}
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
                Template
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Labor
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Other Costs
              </th>
              <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                Actions
              </th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {cakes.map((cake) => (
              <tr key={cake.id}>
                <td className="px-6 py-4 whitespace-nowrap text-gray-900">
                  {cake.name}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-gray-900">
                  {cake.template?.name || 'N/A'}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-gray-900">
                  ${cake.labor.toFixed(2)}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-gray-900">
                  ${cake.otherCosts.toFixed(2)}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  <button
                    onClick={() => handleEdit(cake)}
                    className="text-blue-600 hover:text-blue-900 mr-4"
                  >
                    Edit
                  </button>
                  <button
                    onClick={() => handleDelete(cake.id)}
                    className="text-red-600 hover:text-red-900"
                  >
                    Delete
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {cakes.length === 0 && (
          <div className="text-center py-8 text-gray-500">
            No cakes found. Add one to get started!
          </div>
        )}
      </div>
    </div>
  );
}
