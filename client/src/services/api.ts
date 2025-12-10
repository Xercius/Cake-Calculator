import type { 
  Ingredient, 
  Template, 
  Cake, 
  PricingResult,
  CakeType,
  CakeShape,
  CakeSize,
  Filling,
  Frosting,
  PricingPreviewRequest,
  PricingPreviewResponse,
  Role
} from '../types';

const API_BASE = '/api';

// Ingredients API
export const getIngredients = async (): Promise<Ingredient[]> => {
  const response = await fetch(`${API_BASE}/ingredients`);
  if (!response.ok) throw new Error('Failed to fetch ingredients');
  return response.json();
};

export const getIngredient = async (id: number): Promise<Ingredient> => {
  const response = await fetch(`${API_BASE}/ingredients/${id}`);
  if (!response.ok) throw new Error('Failed to fetch ingredient');
  return response.json();
};

export const createIngredient = async (ingredient: Omit<Ingredient, 'id'>): Promise<Ingredient> => {
  const response = await fetch(`${API_BASE}/ingredients`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(ingredient),
  });
  if (!response.ok) throw new Error('Failed to create ingredient');
  return response.json();
};

export const updateIngredient = async (id: number, ingredient: Omit<Ingredient, 'id'>): Promise<void> => {
  const response = await fetch(`${API_BASE}/ingredients/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(ingredient),
  });
  if (!response.ok) throw new Error('Failed to update ingredient');
};

export const deleteIngredient = async (id: number): Promise<void> => {
  const response = await fetch(`${API_BASE}/ingredients/${id}`, {
    method: 'DELETE',
  });
  if (!response.ok) throw new Error('Failed to delete ingredient');
};

// Templates API
export const getTemplates = async (): Promise<Template[]> => {
  const response = await fetch(`${API_BASE}/templates`);
  if (!response.ok) throw new Error('Failed to fetch templates');
  return response.json();
};

export const getTemplate = async (id: number): Promise<Template> => {
  const response = await fetch(`${API_BASE}/templates/${id}`);
  if (!response.ok) throw new Error('Failed to fetch template');
  return response.json();
};

export const createTemplate = async (template: Omit<Template, 'id'>): Promise<Template> => {
  const response = await fetch(`${API_BASE}/templates`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(template),
  });
  if (!response.ok) throw new Error('Failed to create template');
  return response.json();
};

export const updateTemplate = async (id: number, template: Omit<Template, 'id'>): Promise<void> => {
  const response = await fetch(`${API_BASE}/templates/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(template),
  });
  if (!response.ok) throw new Error('Failed to update template');
};

export const deleteTemplate = async (id: number): Promise<void> => {
  const response = await fetch(`${API_BASE}/templates/${id}`, {
    method: 'DELETE',
  });
  if (!response.ok) throw new Error('Failed to delete template');
};

// Cakes API
export const getCakes = async (): Promise<Cake[]> => {
  const response = await fetch(`${API_BASE}/cakes`);
  if (!response.ok) throw new Error('Failed to fetch cakes');
  return response.json();
};

export const getCake = async (id: number): Promise<Cake> => {
  const response = await fetch(`${API_BASE}/cakes/${id}`);
  if (!response.ok) throw new Error('Failed to fetch cake');
  return response.json();
};

export const createCake = async (cake: Omit<Cake, 'id' | 'template'>): Promise<Cake> => {
  const response = await fetch(`${API_BASE}/cakes`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(cake),
  });
  if (!response.ok) throw new Error('Failed to create cake');
  return response.json();
};

export const updateCake = async (id: number, cake: Omit<Cake, 'id' | 'template'>): Promise<void> => {
  const response = await fetch(`${API_BASE}/cakes/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(cake),
  });
  if (!response.ok) throw new Error('Failed to update cake');
};

export const deleteCake = async (id: number): Promise<void> => {
  const response = await fetch(`${API_BASE}/cakes/${id}`, {
    method: 'DELETE',
  });
  if (!response.ok) throw new Error('Failed to delete cake');
};

// Pricing API
export const getPricing = async (id: number, margins?: string): Promise<PricingResult> => {
  const url = margins 
    ? `${API_BASE}/pricing/${id}?margins=${margins}`
    : `${API_BASE}/pricing/${id}`;
  const response = await fetch(url);
  if (!response.ok) throw new Error('Failed to fetch pricing');
  return response.json();
};

// New Order Settings APIs
export const getCakeTypes = async (): Promise<CakeType[]> => {
  const response = await fetch(`${API_BASE}/caketypes`);
  if (!response.ok) throw new Error('Failed to fetch cake types');
  return response.json();
};

export const getCakeShapes = async (): Promise<CakeShape[]> => {
  const response = await fetch(`${API_BASE}/cakeshapes`);
  if (!response.ok) throw new Error('Failed to fetch cake shapes');
  return response.json();
};

export const getCakeSizes = async (shapeId?: number): Promise<CakeSize[]> => {
  const url = shapeId ? `${API_BASE}/cakesizes?shapeId=${shapeId}` : `${API_BASE}/cakesizes`;
  const response = await fetch(url);
  if (!response.ok) throw new Error('Failed to fetch cake sizes');
  return response.json();
};

export const getFillings = async (): Promise<Filling[]> => {
  const response = await fetch(`${API_BASE}/fillings`);
  if (!response.ok) throw new Error('Failed to fetch fillings');
  return response.json();
};

export const getFrostings = async (): Promise<Frosting[]> => {
  const response = await fetch(`${API_BASE}/frostings`);
  if (!response.ok) throw new Error('Failed to fetch frostings');
  return response.json();
};

export const getPricingPreview = async (request: PricingPreviewRequest): Promise<PricingPreviewResponse> => {
  const response = await fetch(`${API_BASE}/pricing/preview`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(request),
  });
  if (!response.ok) throw new Error('Failed to get pricing preview');
  return response.json();
};

// Roles API
export const getRoles = async (): Promise<Role[]> => {
  const response = await fetch(`${API_BASE}/roles`);
  if (!response.ok) throw new Error('Failed to fetch roles');
  return response.json();
};

export const getRole = async (id: number): Promise<Role> => {
  const response = await fetch(`${API_BASE}/roles/${id}`);
  if (!response.ok) throw new Error('Failed to fetch role');
  return response.json();
};

export const createRole = async (role: Omit<Role, 'id'>): Promise<Role> => {
  const response = await fetch(`${API_BASE}/roles`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(role),
  });
  if (!response.ok) throw new Error('Failed to create role');
  return response.json();
};

export const updateRole = async (id: number, role: Omit<Role, 'id'>): Promise<void> => {
  const response = await fetch(`${API_BASE}/roles/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(role),
  });
  if (!response.ok) throw new Error('Failed to update role');
};

export const deleteRole = async (id: number): Promise<void> => {
  const response = await fetch(`${API_BASE}/roles/${id}`, {
    method: 'DELETE',
  });
  if (!response.ok) throw new Error('Failed to delete role');
};
