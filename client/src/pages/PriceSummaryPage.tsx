import { useState, useEffect } from 'react';
import type { Cake, PricingResult } from '../types';
import * as api from '../services/api';

export default function PriceSummaryPage() {
  const [cakes, setCakes] = useState<Cake[]>([]);
  const [selectedCakeId, setSelectedCakeId] = useState<number>(0);
  const [margins, setMargins] = useState('0.1,0.2,0.3');
  const [pricing, setPricing] = useState<PricingResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    loadCakes();
  }, []);

  const loadCakes = async () => {
    try {
      const data = await api.getCakes();
      setCakes(data);
      setError('');
    } catch (err) {
      setError('Failed to load cakes');
    }
  };

  const handleCalculate = async () => {
    if (!selectedCakeId) {
      setError('Please select a cake');
      return;
    }

    try {
      setLoading(true);
      setError('');
      const result = await api.getPricing(selectedCakeId, margins);
      setPricing(result);
    } catch (err) {
      setError('Failed to calculate pricing');
      setPricing(null);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-4xl mx-auto p-8">
      <h1 className="text-3xl font-bold mb-6">Price Summary</h1>

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}

      <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
        <div className="mb-4">
          <label className="block text-gray-700 text-sm font-bold mb-2">
            Select Cake
          </label>
          <select
            value={selectedCakeId}
            onChange={(e) => setSelectedCakeId(parseInt(e.target.value))}
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          >
            <option value={0}>Choose a cake...</option>
            {cakes.map((cake) => (
              <option key={cake.id} value={cake.id}>
                {cake.name}
              </option>
            ))}
          </select>
        </div>

        <div className="mb-4">
          <label className="block text-gray-700 text-sm font-bold mb-2">
            Margins (comma-separated decimals)
          </label>
          <input
            type="text"
            value={margins}
            onChange={(e) => setMargins(e.target.value)}
            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            placeholder="0.1,0.2,0.3"
          />
          <p className="text-gray-600 text-xs mt-1">
            Example: 0.1,0.2,0.3 for 10%, 20%, and 30% margins
          </p>
        </div>

        <button
          onClick={handleCalculate}
          disabled={loading || !selectedCakeId}
          className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {loading ? 'Calculating...' : 'Calculate Pricing'}
        </button>
      </div>

      {pricing && (
        <div className="bg-white shadow-md rounded px-8 pt-6 pb-8">
          <h2 className="text-2xl font-bold mb-4">{pricing.cakeName}</h2>
          
          <div className="mb-6 p-4 bg-gray-100 rounded">
            <div className="text-lg font-semibold">
              Total Cost: <span className="text-blue-600">${pricing.totalCost.toFixed(2)}</span>
            </div>
          </div>

          <h3 className="text-xl font-bold mb-4">Suggested Prices</h3>
          <div className="space-y-4">
            {pricing.prices.map((priceItem, index) => (
              <div
                key={index}
                className="flex justify-between items-center p-4 border rounded hover:bg-gray-50"
              >
                <div>
                  <div className="font-semibold text-lg">
                    Margin: {(priceItem.margin * 100).toFixed(0)}%
                  </div>
                  <div className="text-sm text-gray-600">
                    Profit: ${(priceItem.price - pricing.totalCost).toFixed(2)}
                  </div>
                </div>
                <div className="text-2xl font-bold text-green-600">
                  ${priceItem.price.toFixed(2)}
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {!pricing && !loading && (
        <div className="bg-gray-50 border border-gray-200 rounded p-8 text-center text-gray-500">
          Select a cake and click "Calculate Pricing" to see the price breakdown
        </div>
      )}
    </div>
  );
}
