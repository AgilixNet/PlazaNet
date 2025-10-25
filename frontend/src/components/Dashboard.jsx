import React from 'react';
import { ShoppingCart, LogOut, LayoutDashboard, Users, BarChart3, Settings } from 'lucide-react';

export default function Dashboard({ user, onLogout }) {
  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900">
      {/* Navbar */}
      <nav className="bg-slate-900/95 backdrop-blur-lg shadow-lg border-b border-white/10">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-16">
            <div className="flex items-center space-x-2">
              <ShoppingCart className="w-8 h-8 text-purple-400" />
              <span className="text-2xl font-bold text-white">PlazaNet</span>
            </div>
            
            <div className="flex items-center gap-4">
              <span className="text-gray-300">Bienvenido, {user?.email}</span>
              <button
                onClick={onLogout}
                className="flex items-center gap-2 bg-red-500/20 hover:bg-red-500/30 text-red-300 px-4 py-2 rounded-lg transition-all"
              >
                <LogOut className="w-4 h-4" />
                Salir
              </button>
            </div>
          </div>
        </div>
      </nav>

      {/* Contenido */}
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        <h1 className="text-4xl font-bold text-white mb-8">Dashboard Principal</h1>
        
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          {/* Card 1 */}
          <div className="bg-white/10 backdrop-blur-lg border border-white/20 rounded-2xl p-6 hover:scale-105 transition-all">
            <div className="flex items-center justify-between mb-4">
              <LayoutDashboard className="w-8 h-8 text-purple-400" />
              <span className="text-2xl font-bold text-white">125</span>
            </div>
            <h3 className="text-gray-300 font-semibold">Locales Activos</h3>
          </div>

          {/* Card 2 */}
          <div className="bg-white/10 backdrop-blur-lg border border-white/20 rounded-2xl p-6 hover:scale-105 transition-all">
            <div className="flex items-center justify-between mb-4">
              <Users className="w-8 h-8 text-blue-400" />
              <span className="text-2xl font-bold text-white">350</span>
            </div>
            <h3 className="text-gray-300 font-semibold">Locatarios</h3>
          </div>

          {/* Card 3 */}
          <div className="bg-white/10 backdrop-blur-lg border border-white/20 rounded-2xl p-6 hover:scale-105 transition-all">
            <div className="flex items-center justify-between mb-4">
              <BarChart3 className="w-8 h-8 text-green-400" />
              <span className="text-2xl font-bold text-white">$45K</span>
            </div>
            <h3 className="text-gray-300 font-semibold">Ventas Mensuales</h3>
          </div>

          {/* Card 4 */}
          <div className="bg-white/10 backdrop-blur-lg border border-white/20 rounded-2xl p-6 hover:scale-105 transition-all">
            <div className="flex items-center justify-between mb-4">
              <Settings className="w-8 h-8 text-orange-400" />
              <span className="text-2xl font-bold text-white">98%</span>
            </div>
            <h3 className="text-gray-300 font-semibold">Ocupación</h3>
          </div>
        </div>

        {/* Sección de bienvenida */}
        <div className="mt-12 bg-gradient-to-r from-purple-600/20 to-pink-600/20 backdrop-blur-xl rounded-3xl p-8 border border-white/20">
          <h2 className="text-3xl font-bold text-white mb-4">¡Bienvenido a PlazaNet!</h2>
          <p className="text-gray-300 text-lg mb-6">
            Estás viendo el dashboard principal. Aquí podrás gestionar todos los aspectos de tu plaza de mercado.
          </p>
          <div className="flex gap-4">
            <button className="bg-gradient-to-r from-purple-500 to-pink-500 text-white px-6 py-3 rounded-lg font-semibold hover:shadow-lg transition-all">
              Explorar Funciones
            </button>
            <button className="border border-purple-400 text-white px-6 py-3 rounded-lg font-semibold hover:bg-purple-400/10 transition-all">
              Ver Tutorial
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}