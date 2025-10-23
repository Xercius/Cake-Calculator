import type { Ingredient, Template, Cake, PricingResult } from '../types';

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
