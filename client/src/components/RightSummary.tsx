import type { PricingPreviewResponse, SelectionState } from '../types';

interface RightSummaryProps {
  selection: SelectionState;
  pricing?: PricingPreviewResponse;
  pricingError?: boolean;
  pricingLoading?: boolean;
  onCreateOrder: () => void;
  isValid: boolean;
}

const formatCurrency = (amount: number): string => {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(amount);
};

export default function RightSummary({
  selection,
  pricing,
  pricingError,
  pricingLoading,
  onCreateOrder,
  isValid,
}: RightSummaryProps) {
  return (
    <div className="bg-white p-6 rounded-lg shadow-md h-fit sticky top-4">
      <h2 className="text-xl font-bold mb-4">Cost Summary</h2>
      
      <div className="space-y-3 mb-6">
        {selection.typeId && (
          <div className="text-sm">
            <span className="text-gray-600">Type:</span>
            <span className="ml-2 font-medium">Selected</span>
          </div>
        )}
        
        {selection.shapeId && (
          <div className="text-sm">
            <span className="text-gray-600">Shape:</span>
            <span className="ml-2 font-medium">Selected</span>
          </div>
        )}
        
        {(selection.sizeId || selection.customSize) && (
          <div className="text-sm">
            <span className="text-gray-600">Size:</span>
            <span className="ml-2 font-medium">
              {selection.sizeId ? 'Preset' : 'Custom'}
            </span>
          </div>
        )}
        
        <div className="text-sm">
          <span className="text-gray-600">Layers:</span>
          <span className="ml-2 font-medium">{selection.layers}</span>
        </div>
        
        {selection.layers > 1 && selection.fillingId && (
          <div className="text-sm">
            <span className="text-gray-600">Filling:</span>
            <span className="ml-2 font-medium">Selected</span>
          </div>
        )}
        
        {selection.frostingId && (
          <div className="text-sm">
            <span className="text-gray-600">Frosting:</span>
            <span className="ml-2 font-medium">Selected</span>
          </div>
        )}
      </div>

      {pricingLoading && (
        <div className="mb-6">
          <div className="animate-pulse space-y-2">
            <div className="h-4 bg-gray-200 rounded w-3/4"></div>
            <div className="h-4 bg-gray-200 rounded w-1/2"></div>
            <div className="h-4 bg-gray-200 rounded w-5/6"></div>
          </div>
        </div>
      )}

      {pricingError && (
        <div className="mb-6 p-3 bg-red-50 border border-red-200 rounded">
          <p className="text-sm text-red-600">Cost unavailable.</p>
        </div>
      )}

      {pricing && !pricingLoading && !pricingError && (
        <div className="mb-6 space-y-2">
          <div className="text-sm flex justify-between">
            <span className="text-gray-600">Ingredients:</span>
            <span>{formatCurrency(pricing.costBreakdown.ingredients)}</span>
          </div>
          <div className="text-sm flex justify-between">
            <span className="text-gray-600">Labor:</span>
            <span>{formatCurrency(pricing.costBreakdown.labor)}</span>
          </div>
          <div className="text-sm flex justify-between">
            <span className="text-gray-600">Overhead:</span>
            <span>{formatCurrency(pricing.costBreakdown.overhead)}</span>
          </div>
          <div className="border-t pt-2 mt-2">
            <div className="flex justify-between font-bold">
              <span>Total Cost:</span>
              <span>{formatCurrency(pricing.totalCost)}</span>
            </div>
          </div>
        </div>
      )}

      <button
        onClick={onCreateOrder}
        disabled={!isValid}
        className={`
          w-full py-3 px-4 rounded-md font-semibold transition-colors
          ${isValid
            ? 'bg-blue-600 text-white hover:bg-blue-700'
            : 'bg-gray-300 text-gray-500 cursor-not-allowed'
          }
        `}
      >
        Create Order
      </button>
    </div>
  );
}
