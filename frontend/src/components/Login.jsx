import React, { useState } from 'react';
import { ShoppingCart, Mail, Lock, Eye, EyeOff, AlertCircle } from 'lucide-react';
import { supabase } from './supabaseClient';

export default function Login({ onLogin }) {
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  const [formData, setFormData] = useState({
    email: '',
    password: ''
  });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const { data, error } = await supabase.auth.signInWithPassword({
        email: formData.email,
        password: formData.password,
      });

      if (error) {
        throw error;
      }

      // Login exitoso
      if (data.user) {
        onLogin(data.user);
      }

    } catch (error) {
      console.error('Error de login:', error);
      
      // Mensajes de error personalizados
      if (error.message.includes('Invalid login credentials')) {
        setError('Correo o contraseña incorrectos');
      } else if (error.message.includes('Email not confirmed')) {
        setError('Por favor confirma tu correo electrónico');
      } else {
        setError(error.message);
      }
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    setFormData(prev => ({
      ...prev,
      [e.target.name]: e.target.value
    }));
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900 flex items-center justify-center p-4">
      {/* Background decorativo */}
      <div className="absolute inset-0 overflow-hidden pointer-events-none">
        <div className="absolute top-1/4 left-1/4 w-96 h-96 bg-purple-500/20 rounded-full blur-3xl"></div>
        <div className="absolute bottom-1/4 right-1/4 w-96 h-96 bg-pink-500/20 rounded-full blur-3xl"></div>
      </div>

      <div className="relative w-full max-w-md">
        {/* Logo */}
        <div className="text-center mb-8">
          <div className="inline-flex items-center space-x-2 mb-4">
            <ShoppingCart className="w-12 h-12 text-purple-400" />
            <span className="text-4xl font-bold text-white">PlazaNet</span>
          </div>
          <p className="text-gray-300">Gestión Inteligente de Plazas de Mercado</p>
        </div>

        {/* Card de Login */}
        <div className="bg-white/10 backdrop-blur-xl border border-white/20 rounded-2xl p-8 shadow-2xl">
          {/* Título */}
          <h2 className="text-2xl font-bold text-white text-center mb-6">
            Iniciar Sesión
          </h2>

          {/* Mensajes de error */}
          {error && (
            <div className="mb-4 p-4 bg-red-500/20 border border-red-500/50 rounded-lg flex items-center gap-2">
              <AlertCircle className="w-5 h-5 text-red-400 flex-shrink-0" />
              <span className="text-red-200">{error}</span>
            </div>
          )}

          {/* Formulario */}
          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="block text-white font-semibold mb-2">
                Correo Electrónico
              </label>
              <div className="relative">
                <Mail className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
                <input
                  type="email"
                  name="email"
                  required
                  value={formData.email}
                  onChange={handleChange}
                  className="w-full pl-12 pr-4 py-3 bg-slate-800/50 text-white border border-white/20 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 placeholder-gray-400"
                  placeholder="correo@ejemplo.com"
                  autoComplete="email"
                />
              </div>
            </div>

            <div>
              <label className="block text-white font-semibold mb-2">
                Contraseña
              </label>
              <div className="relative">
                <Lock className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
                <input
                  type={showPassword ? 'text' : 'password'}
                  name="password"
                  required
                  value={formData.password}
                  onChange={handleChange}
                  className="w-full pl-12 pr-12 py-3 bg-slate-800/50 text-white border border-white/20 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 placeholder-gray-400"
                  placeholder="••••••••"
                  autoComplete="current-password"
                />
                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                  className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-white transition"
                >
                  {showPassword ? <EyeOff className="w-5 h-5" /> : <Eye className="w-5 h-5" />}
                </button>
              </div>
            </div>

            <div className="flex items-center justify-between text-sm">
              <label className="flex items-center text-gray-300 cursor-pointer">
                <input type="checkbox" className="mr-2 rounded" />
                Recordarme
              </label>
              <a href="#" className="text-purple-400 hover:text-purple-300 transition">
                ¿Olvidaste tu contraseña?
              </a>
            </div>

            <button
              type="submit"
              disabled={loading}
              className="w-full bg-gradient-to-r from-purple-500 to-pink-500 text-white py-3 rounded-lg font-bold text-lg hover:shadow-2xl hover:scale-105 transition-all disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:scale-100"
            >
              {loading ? (
                <span className="flex items-center justify-center gap-2">
                  <div className="w-5 h-5 border-2 border-white border-t-transparent rounded-full animate-spin"></div>
                  Iniciando sesión...
                </span>
              ) : (
                'Iniciar Sesión'
              )}
            </button>
          </form>
        </div>

        {/* Footer */}
        <p className="text-center text-gray-400 mt-6 text-sm">
          ¿Necesitas acceso? Contacta al administrador del sistema
        </p>
      </div>
    </div>
  );
}