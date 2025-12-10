export interface Ingredient {
  id: number;
  name: string;
  costPerUnit: number;
}

export interface Template {
  id: number;
  name: string;
  size: string;
  type: string;
  baseIngredients: string;
}

export interface Cake {
  id: number;
  name: string;
  templateId: number;
  template?: Template;
  extraIngredients?: string;
  labor: number;
  otherCosts: number;
}

export interface PricingResult {
  cakeId: number;
  cakeName: string;
  totalCost: number;
  prices: {
    margin: number;
    price: number;
  }[];
}

export interface CakeType {
  id: number;
  name: string;
  imagePath?: string;
  sortOrder: number;
  isActive: boolean;
}

export interface CakeShape {
  id: number;
  name: string;
  imagePath?: string;
  sortOrder: number;
  isActive: boolean;
}

export interface CakeSize {
  id: number;
  shapeId: number;
  name: string;
  dimensions?: string;
  imagePath?: string;
  sortOrder: number;
  isActive: boolean;
}

export interface Filling {
  id: number;
  name: string;
  imagePath?: string;
  sortOrder: number;
  isActive: boolean;
}

export interface Frosting {
  id: number;
  name: string;
  imagePath?: string;
  sortOrder: number;
  isActive: boolean;
}

export interface CustomSize {
  diameterIn?: number;
  lengthIn?: number;
  widthIn?: number;
}

export interface PricingPreviewRequest {
  typeId?: string;
  shapeId?: string;
  sizeId?: string;
  customSize?: CustomSize;
  layers: number;
  fillingId?: string;
  frostingId?: string;
}

export interface CostBreakdown {
  ingredients: number;
  labor: number;
  overhead: number;
}

export interface PricingPreviewResponse {
  costBreakdown: CostBreakdown;
  totalCost: number;
  currency: string;
}

export interface SelectionState {
  typeId?: string;
  shapeId?: string;
  sizeId?: string;
  customSize?: CustomSize;
  layers: number;
  fillingId?: string;
  frostingId?: string;
}

export interface Role {
  id: number;
  name: string;
  hourlyRate: number;
}
