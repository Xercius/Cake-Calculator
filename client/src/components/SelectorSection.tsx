import Tile from './Tile';

interface SelectorItem {
  id: string | number;
  name: string;
  imagePath?: string;
}

interface SelectorSectionProps {
  title: string;
  items: SelectorItem[];
  selectedId?: string | number;
  onSelect: (id: string | number) => void;
  maxPerRow?: number;
}

export default function SelectorSection({
  title,
  items,
  selectedId,
  onSelect,
  maxPerRow = 5,
}: SelectorSectionProps) {
  if (items.length === 0) {
    return (
      <div className="mb-8">
        <h2 className="text-xl font-semibold mb-4">{title}</h2>
        <div className="p-8 bg-gray-50 border border-gray-200 rounded-lg text-center">
          <p className="text-gray-500">No {title} configured.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="mb-8">
      <h2 className="text-xl font-semibold mb-4">{title}</h2>
      <div 
        className="grid gap-4"
        style={{
          gridTemplateColumns: `repeat(auto-fill, minmax(8rem, 1fr))`,
          maxWidth: `${maxPerRow * 9}rem`,
        }}
      >
        {items.map((item) => (
          <Tile
            key={item.id}
            id={item.id}
            label={item.name}
            imagePath={item.imagePath}
            selected={selectedId === item.id}
            onSelect={onSelect}
          />
        ))}
      </div>
    </div>
  );
}
