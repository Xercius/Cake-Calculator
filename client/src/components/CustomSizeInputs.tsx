import type { CustomSize } from '../types';

interface CustomSizeInputsProps {
  shapeName: string;
  customSize?: CustomSize;
  onChange: (customSize: CustomSize) => void;
}

export default function CustomSizeInputs({ shapeName, customSize, onChange }: CustomSizeInputsProps) {
  const isRound = shapeName.toLowerCase() === 'round';

  return (
    <div className="mb-8">
      <h2 className="text-xl font-semibold mb-4">Custom Size</h2>
      <div className="flex gap-4">
        {isRound ? (
          <div className="flex flex-col">
            <label htmlFor="diameter" className="text-sm font-medium mb-1">
              Diameter (inches)
            </label>
            <input
              type="number"
              id="diameter"
              min="1"
              step="0.5"
              value={customSize?.diameterIn ?? ''}
              onChange={(e) => onChange({ diameterIn: parseFloat(e.target.value) || undefined })}
              className="w-40 px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Enter diameter"
            />
          </div>
        ) : (
          <>
            <div className="flex flex-col">
              <label htmlFor="length" className="text-sm font-medium mb-1">
                Length (inches)
              </label>
              <input
                type="number"
                id="length"
                min="1"
                step="0.5"
                value={customSize?.lengthIn ?? ''}
                onChange={(e) => 
                  onChange({ 
                    ...(customSize || {}), 
                    lengthIn: parseFloat(e.target.value) || undefined 
                  })
                }
                className="w-40 px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Enter length"
              />
            </div>
            <div className="flex flex-col">
              <label htmlFor="width" className="text-sm font-medium mb-1">
                Width (inches)
              </label>
              <input
                type="number"
                id="width"
                min="1"
                step="0.5"
                value={customSize?.widthIn ?? ''}
                onChange={(e) => 
                  onChange({ 
                    ...(customSize || {}), 
                    widthIn: parseFloat(e.target.value) || undefined 
                  })
                }
                className="w-40 px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Enter width"
              />
            </div>
          </>
        )}
      </div>
    </div>
  );
}
