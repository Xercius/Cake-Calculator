import { useState } from 'react';

interface TileProps {
  id: string | number;
  label: string;
  imagePath?: string;
  selected: boolean;
  onSelect: (id: string | number) => void;
}

export default function Tile({ id, label, imagePath, selected, onSelect }: TileProps) {
  const [imageError, setImageError] = useState(false);

  return (
    <button
      onClick={() => onSelect(id)}
      className={`
        relative flex flex-col items-center justify-center
        w-32 h-32 rounded-lg border-2 cursor-pointer
        transition-all duration-200 ease-in-out
        ${selected 
          ? 'border-blue-600 scale-110' 
          : 'border-gray-300 opacity-80 hover:border-gray-400'
        }
        focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2
      `}
      aria-pressed={selected}
    >
      <div className="flex flex-col items-center justify-center h-full w-full p-2">
        {imagePath && !imageError ? (
          <img 
            src={imagePath} 
            alt={label}
            className="w-16 h-16 object-cover mb-2"
            onError={() => setImageError(true)}
          />
        ) : (
          <div className="w-16 h-16 bg-gray-200 rounded flex items-center justify-center mb-2">
            <span className="text-2xl text-gray-400">?</span>
          </div>
        )}
        <span className="text-sm font-medium text-center line-clamp-2">
          {label}
        </span>
      </div>
    </button>
  );
}
