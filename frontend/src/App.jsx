import { useState, useEffect } from 'react';
import LandingPage from './components/LandingPage';
import Login from './components/Login';
import Dashboard from './components/Dashboard';
import { supabase } from './supabaseClient';

function App() {
  const [currentView, setCurrentView] = useState('landing');
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  // Verificar sesión al cargar la app
  useEffect(() => {
    checkSession();

    // Escuchar cambios en la autenticación
    const { data: { subscription } } = supabase.auth.onAuthStateChange((_event, session) => {
      setUser(session?.user || null);
      if (session?.user) {
        setCurrentView('dashboard');
      } else {
        setCurrentView('landing');
      }
    });

    return () => subscription.unsubscribe();
  }, []);

  const checkSession = async () => {
    try {
      const { data: { session } } = await supabase.auth.getSession();
      setUser(session?.user || null);
      if (session?.user) {
        setCurrentView('dashboard');
      }
    } catch (error) {
      console.error('Error checking session:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleLogin = (userData) => {
    setUser(userData);
    setCurrentView('dashboard');
  };

  const handleLogout = async () => {
    try {
      await supabase.auth.signOut();
      setUser(null);
      setCurrentView('landing');
    } catch (error) {
      console.error('Error al cerrar sesión:', error);
    }
  };

  const handleShowLogin = () => {
    setCurrentView('login');
  };

  // Mostrar loading mientras se verifica la sesión
  if (loading) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900 flex items-center justify-center">
        <div className="text-center">
          <div className="w-16 h-16 border-4 border-purple-500 border-t-transparent rounded-full animate-spin mx-auto mb-4"></div>
          <p className="text-white text-lg">Cargando...</p>
        </div>
      </div>
    );
  }

  return (
    <>
      {currentView === 'landing' && <LandingPage onShowLogin={handleShowLogin} />}
      {currentView === 'login' && <Login onLogin={handleLogin} />}
      {currentView === 'dashboard' && <Dashboard user={user} onLogout={handleLogout} />}
    </>
  );
}

export default App;