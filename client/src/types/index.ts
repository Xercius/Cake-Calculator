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
