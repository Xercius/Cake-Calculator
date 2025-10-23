import { useState } from 'react';
import IngredientsPage from './pages/IngredientsPage';
import TemplatesPage from './pages/TemplatesPage';
import CakesPage from './pages/CakesPage';
import PriceSummaryPage from './pages/PriceSummaryPage';

type Page = 'ingredients' | 'templates' | 'cakes' | 'pricing';

function App() {
  const [currentPage, setCurrentPage] = useState<Page>('ingredients');

  const renderPage = () => {
    switch (currentPage) {
      case 'ingredients':
        return <IngredientsPage />;
      case 'templates':
        return <TemplatesPage />;
      case 'cakes':
        return <CakesPage />;
      case 'pricing':
        return <PriceSummaryPage />;
      default:
        return <IngredientsPage />;
    }
  };

  return (
    <div className="min-h-screen bg-gray-100">
      <nav className="bg-white shadow-lg">
        <div className="max-w-7xl mx-auto px-4">
          <div className="flex justify-between">
            <div className="flex space-x-4">
              <div className="flex items-center py-5 px-2">
                <span className="font-bold text-xl text-gray-800">Cake Calculator</span>
              </div>
              <div className="flex items-center space-x-1">
                <button
                  onClick={() => setCurrentPage('ingredients')}
                  className={`py-5 px-3 ${
                    currentPage === 'ingredients'
                      ? 'text-blue-600 border-b-4 border-blue-600'
                      : 'text-gray-500 hover:text-blue-600'
                  } transition duration-300`}
                >
                  Ingredients
                </button>
                <button
                  onClick={() => setCurrentPage('templates')}
                  className={`py-5 px-3 ${
                    currentPage === 'templates'
                      ? 'text-blue-600 border-b-4 border-blue-600'
                      : 'text-gray-500 hover:text-blue-600'
                  } transition duration-300`}
                >
                  Templates
                </button>
                <button
                  onClick={() => setCurrentPage('cakes')}
                  className={`py-5 px-3 ${
                    currentPage === 'cakes'
                      ? 'text-blue-600 border-b-4 border-blue-600'
                      : 'text-gray-500 hover:text-blue-600'
                  } transition duration-300`}
                >
                  Cakes
                </button>
                <button
                  onClick={() => setCurrentPage('pricing')}
                  className={`py-5 px-3 ${
                    currentPage === 'pricing'
                      ? 'text-blue-600 border-b-4 border-blue-600'
                      : 'text-gray-500 hover:text-blue-600'
                  } transition duration-300`}
                >
                  Price Summary
                </button>
              </div>
            </div>
          </div>
        </div>
      </nav>
      <main>{renderPage()}</main>
    </div>
  );
}

export default App;

