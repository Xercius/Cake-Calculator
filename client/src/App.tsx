import { BrowserRouter, Routes, Route, Link, useLocation } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import IngredientsPage from './pages/IngredientsPage';
import TemplatesPage from './pages/TemplatesPage';
import CakesPage from './pages/CakesPage';
import PriceSummaryPage from './pages/PriceSummaryPage';
import NewOrderPage from './pages/NewOrderPage';

const queryClient = new QueryClient();

function Navigation() {
  const location = useLocation();
  
  // Don't show nav on new order page
  if (location.pathname === '/new') {
    return null;
  }
  
  return (
    <nav className="bg-white shadow-lg">
      <div className="max-w-7xl mx-auto px-4">
        <div className="flex justify-between">
          <div className="flex space-x-4">
            <div className="flex items-center py-5 px-2">
              <span className="font-bold text-xl text-gray-800">Cake Calculator</span>
            </div>
            <div className="flex items-center space-x-1">
              <Link
                to="/"
                className={`py-5 px-3 ${
                  location.pathname === '/'
                    ? 'text-blue-600 border-b-4 border-blue-600'
                    : 'text-gray-500 hover:text-blue-600'
                } transition duration-300`}
              >
                Ingredients
              </Link>
              <Link
                to="/templates"
                className={`py-5 px-3 ${
                  location.pathname === '/templates'
                    ? 'text-blue-600 border-b-4 border-blue-600'
                    : 'text-gray-500 hover:text-blue-600'
                } transition duration-300`}
              >
                Templates
              </Link>
              <Link
                to="/cakes"
                className={`py-5 px-3 ${
                  location.pathname === '/cakes'
                    ? 'text-blue-600 border-b-4 border-blue-600'
                    : 'text-gray-500 hover:text-blue-600'
                } transition duration-300`}
              >
                Cakes
              </Link>
              <Link
                to="/pricing"
                className={`py-5 px-3 ${
                  location.pathname === '/pricing'
                    ? 'text-blue-600 border-b-4 border-blue-600'
                    : 'text-gray-500 hover:text-blue-600'
                } transition duration-300`}
              >
                Price Summary
              </Link>
              <Link
                to="/new"
                className={`py-5 px-3 ${
                  location.pathname === '/new'
                    ? 'text-blue-600 border-b-4 border-blue-600'
                    : 'text-gray-500 hover:text-blue-600'
                } transition duration-300`}
              >
                New Order
              </Link>
            </div>
          </div>
        </div>
      </div>
    </nav>
  );
}

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <div className="min-h-screen bg-gray-100">
          <Navigation />
          <main>
            <Routes>
              <Route path="/" element={<IngredientsPage />} />
              <Route path="/templates" element={<TemplatesPage />} />
              <Route path="/cakes" element={<CakesPage />} />
              <Route path="/pricing" element={<PriceSummaryPage />} />
              <Route path="/new" element={<NewOrderPage />} />
            </Routes>
          </main>
        </div>
      </BrowserRouter>
    </QueryClientProvider>
  );
}

export default App;

