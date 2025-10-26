import { useState, useEffect } from 'react';
import { useQuery } from '@tanstack/react-query';
import SelectorSection from '../components/SelectorSection';
import CustomSizeInputs from '../components/CustomSizeInputs';
import RightSummary from '../components/RightSummary';
import { useSelectionQuerySync } from '../hooks/useSelectionQuerySync';
import { usePricePreview } from '../hooks/usePricePreview';
import {
  getCakeTypes,
  getCakeShapes,
  getCakeSizes,
  getFillings,
  getFrostings,
} from '../services/api';
import type { CakeShape } from '../types';

export default function NewOrderPage() {
  const [selection, setSelection] = useSelectionQuerySync();
  const [showCustomSize, setShowCustomSize] = useState(false);
  const [selectedShape, setSelectedShape] = useState<CakeShape | undefined>();
  const [sidebarVisible, setSidebarVisible] = useState(true);

  // Fetch all settings data
  const { data: cakeTypes = [] } = useQuery({
    queryKey: ['cakeTypes'],
    queryFn: getCakeTypes,
  });

  const { data: cakeShapes = [] } = useQuery({
    queryKey: ['cakeShapes'],
    queryFn: getCakeShapes,
  });

  const { data: cakeSizes = [] } = useQuery({
    queryKey: ['cakeSizes', selection.shapeId],
    queryFn: () => getCakeSizes(selection.shapeId ? parseInt(selection.shapeId) : undefined),
    enabled: !!selection.shapeId,
  });

  const { data: fillings = [] } = useQuery({
    queryKey: ['fillings'],
    queryFn: getFillings,
  });

  const { data: frostings = [] } = useQuery({
    queryKey: ['frostings'],
    queryFn: getFrostings,
  });

  // Get pricing preview
  const { pricing, loading: pricingLoading, error: pricingError } = usePricePreview(selection);

  // Track selected shape
  useEffect(() => {
    if (selection.shapeId) {
      const shape = cakeShapes.find(s => s.id.toString() === selection.shapeId);
      setSelectedShape(shape);
    } else {
      setSelectedShape(undefined);
    }
  }, [selection.shapeId, cakeShapes]);

  // Handle viewport size changes
  useEffect(() => {
    const handleResize = () => {
      setSidebarVisible(window.innerWidth >= 1024);
    };

    handleResize();
    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  // Check if all required fields are filled
  const isValid =
    !!selection.typeId &&
    !!selection.shapeId &&
    (!!selection.sizeId || (!!selection.customSize && 
      (!!selection.customSize.diameterIn || 
       (!!selection.customSize.lengthIn && !!selection.customSize.widthIn)))) &&
    (selection.layers === 1 || !!selection.fillingId) &&
    !!selection.frostingId;

  const handleCreateOrder = () => {
    console.log('Create Order:', selection);
    alert('Order created! (Placeholder action)');
  };

  // Prepare size items including custom option
  const sizeItems = [
    ...cakeSizes.map(size => ({
      id: size.id,
      name: size.name,
      imagePath: size.imagePath,
    })),
    {
      id: 'custom',
      name: 'Custom Size',
      imagePath: undefined,
    },
  ];

  // Layer options
  const layerItems = [
    { id: '1', name: '1 Layer' },
    { id: '2', name: '2 Layers' },
    { id: '3', name: '3 Layers' },
  ];

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white shadow-sm">
        <div className="max-w-7xl mx-auto px-4 py-4 flex items-center">
          <button 
            className="mr-4 p-2 hover:bg-gray-100 rounded"
            onClick={() => alert('Menu placeholder')}
          >
            <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
            </svg>
          </button>
          <h1 className="text-2xl font-bold">Cake Calculator - New Order</h1>
        </div>
      </header>

      {/* Main Content */}
      <div className="max-w-7xl mx-auto px-4 py-8">
        <div className="flex gap-8">
          {/* Left: Selectors (80%) */}
          <div className="flex-1">
            {/* Cake Type */}
            <SelectorSection
              title="Cake Type"
              items={cakeTypes}
              selectedId={selection.typeId}
              onSelect={(id) => setSelection({ ...selection, typeId: id.toString() })}
            />

            {/* Cake Shape */}
            <SelectorSection
              title="Cake Shape"
              items={cakeShapes}
              selectedId={selection.shapeId}
              onSelect={(id) => {
                setSelection({ 
                  ...selection, 
                  shapeId: id.toString(),
                  sizeId: undefined,
                  customSize: undefined,
                });
                setShowCustomSize(false);
              }}
            />

            {/* Cake Size (only show after shape selected) */}
            {selection.shapeId && (
              <>
                <SelectorSection
                  title="Size"
                  items={sizeItems}
                  selectedId={showCustomSize ? 'custom' : selection.sizeId}
                  onSelect={(id) => {
                    if (id === 'custom') {
                      setShowCustomSize(true);
                      setSelection({ ...selection, sizeId: undefined, customSize: undefined });
                    } else {
                      setShowCustomSize(false);
                      setSelection({ ...selection, sizeId: id.toString(), customSize: undefined });
                    }
                  }}
                />

                {showCustomSize && selectedShape && (
                  <CustomSizeInputs
                    shapeName={selectedShape.name}
                    customSize={selection.customSize}
                    onChange={(customSize) => setSelection({ ...selection, customSize })}
                  />
                )}
              </>
            )}

            {/* Layers */}
            <SelectorSection
              title="Layers"
              items={layerItems}
              selectedId={selection.layers.toString()}
              onSelect={(id) => {
                const layers = parseInt(id.toString());
                setSelection({ 
                  ...selection, 
                  layers,
                  fillingId: layers === 1 ? undefined : selection.fillingId,
                });
              }}
            />

            {/* Filling (only show if layers > 1) */}
            {selection.layers > 1 && (
              <SelectorSection
                title="Filling"
                items={fillings}
                selectedId={selection.fillingId}
                onSelect={(id) => setSelection({ ...selection, fillingId: id.toString() })}
              />
            )}

            {/* Frosting */}
            <SelectorSection
              title="Frosting"
              items={frostings}
              selectedId={selection.frostingId}
              onSelect={(id) => setSelection({ ...selection, frostingId: id.toString() })}
            />
          </div>

          {/* Right: Cost Summary (20%) */}
          {sidebarVisible && (
            <div className="w-80">
              <RightSummary
                selection={selection}
                pricing={pricing}
                pricingError={pricingError}
                pricingLoading={pricingLoading}
                onCreateOrder={handleCreateOrder}
                isValid={isValid}
              />
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
