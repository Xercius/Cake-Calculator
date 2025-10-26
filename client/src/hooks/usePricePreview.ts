import { useEffect, useState } from 'react';
import type { SelectionState, PricingPreviewResponse } from '../types';
import { getPricingPreview } from '../services/api';

export function usePricePreview(selection: SelectionState) {
  const [pricing, setPricing] = useState<PricingPreviewResponse | undefined>();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(false);

  useEffect(() => {
    // Only fetch pricing if we have enough information
    const hasRequiredFields = 
      selection.typeId &&
      selection.shapeId &&
      (selection.sizeId || selection.customSize) &&
      selection.frostingId;

    if (!hasRequiredFields) {
      setPricing(undefined);
      setError(false);
      return;
    }

    const fetchPricing = async () => {
      setLoading(true);
      setError(false);
      
      try {
        const result = await getPricingPreview({
          typeId: selection.typeId,
          shapeId: selection.shapeId,
          sizeId: selection.sizeId,
          customSize: selection.customSize,
          layers: selection.layers,
          fillingId: selection.fillingId,
          frostingId: selection.frostingId,
        });
        
        setPricing(result);
      } catch (err) {
        console.error('Failed to fetch pricing:', err);
        setError(true);
        setPricing(undefined);
      } finally {
        setLoading(false);
      }
    };

    fetchPricing();
  }, [
    selection.typeId,
    selection.shapeId,
    selection.sizeId,
    selection.customSize?.diameterIn,
    selection.customSize?.lengthIn,
    selection.customSize?.widthIn,
    selection.layers,
    selection.fillingId,
    selection.frostingId,
  ]);

  return { pricing, loading, error };
}
