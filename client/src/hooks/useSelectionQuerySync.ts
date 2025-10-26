import { useEffect, useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import type { SelectionState, CustomSize } from '../types';

export function useSelectionQuerySync() {
  const [searchParams, setSearchParams] = useSearchParams();
  
  const [selection, setSelection] = useState<SelectionState>(() => {
    const typeId = searchParams.get('type') || undefined;
    const shapeId = searchParams.get('shape') || undefined;
    const sizeId = searchParams.get('size') === 'custom' ? undefined : searchParams.get('size') || undefined;
    const isCustom = searchParams.get('size') === 'custom' || searchParams.get('custom') === '1';
    
    let customSize: CustomSize | undefined;
    if (isCustom) {
      const d = searchParams.get('d');
      const l = searchParams.get('l');
      const w = searchParams.get('w');
      customSize = {
        diameterIn: d ? parseFloat(d) : undefined,
        lengthIn: l ? parseFloat(l) : undefined,
        widthIn: w ? parseFloat(w) : undefined,
      };
      // Only set customSize if at least one dimension is present
      if (!customSize.diameterIn && !customSize.lengthIn && !customSize.widthIn) {
        customSize = undefined;
      }
    }
    
    const layers = parseInt(searchParams.get('layers') || '1', 10);
    const fillingId = searchParams.get('filling') || undefined;
    const frostingId = searchParams.get('frosting') || undefined;
    
    return {
      typeId,
      shapeId,
      sizeId,
      customSize,
      layers,
      fillingId,
      frostingId,
    };
  });

  useEffect(() => {
    const params = new URLSearchParams();
    
    if (selection.typeId) params.set('type', selection.typeId);
    if (selection.shapeId) params.set('shape', selection.shapeId);
    
    if (selection.sizeId) {
      params.set('size', selection.sizeId);
    } else if (selection.customSize) {
      params.set('size', 'custom');
      params.set('custom', '1');
      if (selection.customSize.diameterIn) {
        params.set('d', selection.customSize.diameterIn.toString());
      }
      if (selection.customSize.lengthIn) {
        params.set('l', selection.customSize.lengthIn.toString());
      }
      if (selection.customSize.widthIn) {
        params.set('w', selection.customSize.widthIn.toString());
      }
    }
    
    if (selection.layers > 1) {
      params.set('layers', selection.layers.toString());
    }
    
    if (selection.fillingId) params.set('filling', selection.fillingId);
    if (selection.frostingId) params.set('frosting', selection.frostingId);
    
    setSearchParams(params, { replace: true });
  }, [selection, setSearchParams]);

  return [selection, setSelection] as const;
}
