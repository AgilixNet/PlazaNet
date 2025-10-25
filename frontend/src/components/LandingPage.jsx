import React, { useState, useEffect } from 'react';
import { ShoppingCart, Users, BarChart3, Package, CheckCircle, Menu, X, ArrowRight, Zap, Shield, TrendingUp, Upload, FileText, CheckCircle2 } from 'lucide-react';
import { supabase } from '../supabaseClient';

export default function LandingPage({ onShowLogin }) {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [scrolled, setScrolled] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [selectedPlan, setSelectedPlan] = useState('');
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  
  const [formData, setFormData] = useState({
    nombre_representante: '',
    email: '',
    telefono: '',
    nombre_plaza: '',
    tipo_suscripcion: ''
  });
  
  const [files, setFiles] = useState({
    cedula: null,
    rut: null
  });

  useEffect(() => {
    const handleScroll = () => {
      setScrolled(window.scrollY > 50);
    };
    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  const plans = [
    {
      id: 'basico',
      name: 'Básico',
      price: '$49',
      period: '/mes',
      features: [
        'Hasta 20 locales',
        'Gestión básica de ventas',
        'Reportes mensuales',
        'Soporte por email',
        '1 usuario administrador'
      ],
      color: 'from-blue-500 to-cyan-500'
    },
    {
      id: 'pro',
      name: 'Pro',
      price: '$99',
      period: '/mes',
      features: [
        'Hasta 50 locales',
        'Gestión avanzada completa',
        'Reportes en tiempo real',
        'Soporte prioritario 24/7',
        '5 usuarios administradores',
        'Analíticas avanzadas',
        'App móvil incluida'
      ],
      color: 'from-purple-500 to-pink-500',
      popular: true
    },
    {
      id: 'full',
      name: 'Full',
      price: '$199',
      period: '/mes',
      features: [
        'Locales ilimitados',
        'Todo lo de Pro +',
        'API personalizada',
        'Gerente de cuenta dedicado',
        'Usuarios ilimitados',
        'Integraciones personalizadas',
        'Capacitación in-situ',
        'Personalización completa'
      ],
      color: 'from-orange-500 to-red-500'
    }
  ];

  const handleFileChange = (e, type) => {
    const file = e.target.files[0];
    if (file) {
      setFiles(prev => ({ ...prev, [type]: file }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      // 1. Subir archivos a Supabase Storage
      let cedulaUrl = null;
      let rutUrl = null;

      if (files.cedula) {
        const fileName = `cedula_${Date.now()}_${files.cedula.name}`;
        const { data, error } = await supabase.storage
          .from('documentos')
          .upload(fileName, files.cedula);
        
        if (error) throw error;
        cedulaUrl = supabase.storage.from('documentos').getPublicUrl(fileName).data.publicUrl;
      }

      if (files.rut) {
        const fileName = `rut_${Date.now()}_${files.rut.name}`;
        const { data, error } = await supabase.storage
          .from('documentos')
          .upload(fileName, files.rut);
        
        if (error) throw error;
        rutUrl = supabase.storage.from('documentos').getPublicUrl(fileName).data.publicUrl;
      }

      // 2. Guardar datos en la base de datos
      const { data, error } = await supabase
        .from('solicitudes')
        .insert([
          {
            nombre_representante: formData.nombre_representante,
            email: formData.email,
            telefono: formData.telefono,
            nombre_plaza: formData.nombre_plaza,
            tipo_suscripcion: formData.tipo_suscripcion,
            cedula_url: cedulaUrl,
            rut_url: rutUrl
          }
        ]);

      if (error) throw error;

      setSuccess(true);
      setTimeout(() => {
        setShowModal(false);
        setSuccess(false);
        setFormData({
          nombre_representante: '',
          email: '',
          telefono: '',
          nombre_plaza: '',
          tipo_suscripcion: ''
        });
        setFiles({ cedula: null, rut: null });
      }, 3000);

    } catch (error) {
      console.error('Error:', error);
      alert('Hubo un error al enviar la solicitud. Por favor intenta de nuevo.');
    } finally {
      setLoading(false);
    }
  };

  const openModal = (planId) => {
    setSelectedPlan(planId);
    setFormData(prev => ({ ...prev, tipo_suscripcion: planId }));
    setShowModal(true);
  };

  const features = [
    {
      icon: <ShoppingCart className="w-8 h-8" />,
      title: "Gestión de Ventas",
      description: "Control total de ventas, inventarios y transacciones en tiempo real"
    },
    {
      icon: <Users className="w-8 h-8" />,
      title: "Administración de Locatarios",
      description: "Gestiona todos tus comerciantes y sus pagos desde un solo lugar"
    },
    {
      icon: <BarChart3 className="w-8 h-8" />,
      title: "Reportes y Analíticas",
      description: "Dashboards inteligentes con métricas clave de tu plaza"
    },
    {
      icon: <Package className="w-8 h-8" />,
      title: "Control de Inventario",
      description: "Seguimiento automático de productos y stock en cada local"
    }
  ];

  const benefits = [
    "Reduce tiempos administrativos en un 70%",
    "Aumenta la transparencia operativa",
    "Digitaliza todos tus procesos",
    "Acceso desde cualquier dispositivo",
    "Soporte técnico 24/7",
    "Actualizaciones automáticas"
  ];

  const testimonials = [
    {
      name: "María González",
      role: "Administradora Plaza Central",
      text: "PlazaNet transformó completamente nuestra operación. Ahora todo es más eficiente y transparente."
    },
    {
      name: "Carlos Ramírez",
      role: "Director Mercado Norte",
      text: "La mejor inversión que hemos hecho. El retorno fue inmediato en productividad."
    },
    {
      name: "Ana Martínez",
      role: "Gerente Plaza del Sur",
      text: "Increíble cómo simplificó nuestra gestión diaria. Los reportes son espectaculares."
    }
  ];

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900">
      {/* Modal de Solicitud */}
      {showModal && (
        <div className="fixed inset-0 bg-black/70 backdrop-blur-sm z-50 flex items-center justify-center p-4">
          <div className="bg-slate-800 rounded-2xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
            <div className="p-6 border-b border-white/10 flex justify-between items-center sticky top-0 bg-slate-800 z-10">
              <h3 className="text-2xl font-bold text-white">
                Solicitar Plan {plans.find(p => p.id === selectedPlan)?.name}
              </h3>
              <button onClick={() => setShowModal(false)} className="text-gray-400 hover:text-white">
                <X className="w-6 h-6" />
              </button>
            </div>

            {success ? (
              <div className="p-12 text-center">
                <CheckCircle2 className="w-20 h-20 text-green-400 mx-auto mb-4" />
                <h3 className="text-2xl font-bold text-white mb-2">¡Solicitud Enviada!</h3>
                <p className="text-gray-300">Nos pondremos en contacto contigo pronto.</p>
              </div>
            ) : (
              <form onSubmit={handleSubmit} className="p-6 space-y-6">
                <div>
                  <label className="block text-white font-semibold mb-2">
                    Nombre del Representante Legal *
                  </label>
                  <input
                    type="text"
                    required
                    value={formData.nombre_representante}
                    onChange={(e) => setFormData(prev => ({ ...prev, nombre_representante: e.target.value }))}
                    className="w-full px-4 py-3 bg-slate-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
                    placeholder="Juan Pérez"
                  />
                </div>

                <div>
                  <label className="block text-white font-semibold mb-2">
                    Nombre de la Plaza de Mercado *
                  </label>
                  <input
                    type="text"
                    required
                    value={formData.nombre_plaza}
                    onChange={(e) => setFormData(prev => ({ ...prev, nombre_plaza: e.target.value }))}
                    className="w-full px-4 py-3 bg-slate-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
                    placeholder="Plaza Central"
                  />
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-white font-semibold mb-2">
                      Email *
                    </label>
                    <input
                      type="email"
                      required
                      value={formData.email}
                      onChange={(e) => setFormData(prev => ({ ...prev, email: e.target.value }))}
                      className="w-full px-4 py-3 bg-slate-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
                      placeholder="email@ejemplo.com"
                    />
                  </div>

                  <div>
                    <label className="block text-white font-semibold mb-2">
                      Teléfono
                    </label>
                    <input
                      type="tel"
                      value={formData.telefono}
                      onChange={(e) => setFormData(prev => ({ ...prev, telefono: e.target.value }))}
                      className="w-full px-4 py-3 bg-slate-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
                      placeholder="+57 300 123 4567"
                    />
                  </div>
                </div>

                <div>
                  <label className="block text-white font-semibold mb-2">
                    Copia de la Cédula *
                  </label>
                  <div className="border-2 border-dashed border-slate-600 rounded-lg p-6 text-center hover:border-purple-500 transition-colors">
                    <input
                      type="file"
                      id="cedula"
                      required
                      accept=".pdf,.jpg,.jpeg,.png"
                      onChange={(e) => handleFileChange(e, 'cedula')}
                      className="hidden"
                    />
                    <label htmlFor="cedula" className="cursor-pointer">
                      <Upload className="w-12 h-12 text-gray-400 mx-auto mb-2" />
                      <p className="text-white font-semibold">
                        {files.cedula ? files.cedula.name : 'Subir cédula'}
                      </p>
                      <p className="text-gray-400 text-sm">PDF, JPG o PNG (máx. 5MB)</p>
                    </label>
                  </div>
                </div>

                <div>
                  <label className="block text-white font-semibold mb-2">
                    RUT de la Plaza de Mercado *
                  </label>
                  <div className="border-2 border-dashed border-slate-600 rounded-lg p-6 text-center hover:border-purple-500 transition-colors">
                    <input
                      type="file"
                      id="rut"
                      required
                      accept=".pdf,.jpg,.jpeg,.png"
                      onChange={(e) => handleFileChange(e, 'rut')}
                      className="hidden"
                    />
                    <label htmlFor="rut" className="cursor-pointer">
                      <FileText className="w-12 h-12 text-gray-400 mx-auto mb-2" />
                      <p className="text-white font-semibold">
                        {files.rut ? files.rut.name : 'Subir RUT'}
                      </p>
                      <p className="text-gray-400 text-sm">PDF, JPG o PNG (máx. 5MB)</p>
                    </label>
                  </div>
                </div>

                <button
                  type="submit"
                  disabled={loading}
                  className="w-full bg-gradient-to-r from-purple-500 to-pink-500 text-white px-6 py-4 rounded-lg font-bold text-lg hover:shadow-2xl hover:scale-105 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  {loading ? 'Enviando...' : 'Enviar Solicitud'}
                </button>
              </form>
            )}
          </div>
        </div>
      )}

      {/* Navbar */}
      <nav className={`fixed w-full z-40 transition-all duration-300 ${scrolled ? 'bg-slate-900/95 backdrop-blur-lg shadow-lg' : 'bg-transparent'}`}>
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-16">
            <div className="flex items-center space-x-2">
              <ShoppingCart className="w-8 h-8 text-purple-400" />
              <span className="text-2xl font-bold text-white">PlazaNet</span>
            </div>
            
            <div className="hidden md:flex space-x-8">
              <a href="#features" className="text-gray-300 hover:text-white transition">Características</a>
              <a href="#benefits" className="text-gray-300 hover:text-white transition">Beneficios</a>
              <a href="#pricing" className="text-gray-300 hover:text-white transition">Precios</a>
              <a href="#testimonials" className="text-gray-300 hover:text-white transition">Testimonios</a>
            </div>

            <div className="hidden md:block">
              <button 
                onClick={onShowLogin}
                className="bg-gradient-to-r from-purple-500 to-pink-500 text-white px-6 py-2 rounded-full font-semibold hover:shadow-lg hover:scale-105 transition-all"
              >
                Iniciar Sesión
              </button>
            </div>

            <button 
              className="md:hidden text-white"
              onClick={() => setIsMenuOpen(!isMenuOpen)}
            >
              {isMenuOpen ? <X /> : <Menu />}
            </button>
          </div>
        </div>

        {isMenuOpen && (
          <div className="md:hidden bg-slate-800/95 backdrop-blur-lg">
            <div className="px-2 pt-2 pb-3 space-y-1">
              <a href="#features" className="block px-3 py-2 text-gray-300 hover:text-white">Características</a>
              <a href="#benefits" className="block px-3 py-2 text-gray-300 hover:text-white">Beneficios</a>
              <a href="#pricing" className="block px-3 py-2 text-gray-300 hover:text-white">Precios</a>
              <a href="#testimonials" className="block px-3 py-2 text-gray-300 hover:text-white">Testimonios</a>
              <button 
                onClick={onShowLogin}
                className="w-full mt-2 bg-gradient-to-r from-purple-500 to-pink-500 text-white px-6 py-2 rounded-full font-semibold"
              >
                Iniciar Sesión
              </button>
            </div>
          </div>
        )}
      </nav>

      {/* Hero Section */}
      <section className="pt-32 pb-20 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto text-center">
          <div>
            <h1 className="text-5xl md:text-7xl font-bold text-white mb-6 leading-tight">
              Gestiona tu Plaza de Mercado
              <span className="block bg-gradient-to-r from-purple-400 to-pink-400 bg-clip-text text-transparent">
                de Forma Inteligente
              </span>
            </h1>
            <p className="text-xl md:text-2xl text-gray-300 mb-12 max-w-3xl mx-auto">
              PlazaNet es la solución integral que moderniza la gestión operativa de tu plaza de mercado, automatizando procesos y aumentando la eficiencia.
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <button 
                onClick={() => document.getElementById('pricing').scrollIntoView({ behavior: 'smooth' })}
                className="bg-gradient-to-r from-purple-500 to-pink-500 text-white px-8 py-4 rounded-full font-bold text-lg hover:shadow-2xl hover:scale-105 transition-all flex items-center justify-center gap-2"
              >
                Comenzar Ahora <ArrowRight className="w-5 h-5" />
              </button>
              <button 
                onClick={onShowLogin}
                className="border-2 border-purple-400 text-white px-8 py-4 rounded-full font-bold text-lg hover:bg-purple-400/10 transition-all"
              >
                Acceder al Sistema
              </button>
            </div>
          </div>

          <div className="mt-16 flex flex-wrap justify-center gap-6">
            <div className="flex items-center gap-2 bg-white/10 backdrop-blur-lg px-6 py-3 rounded-full">
              <Zap className="w-5 h-5 text-yellow-400" />
              <span className="text-white font-semibold">Rápido</span>
            </div>
            <div className="flex items-center gap-2 bg-white/10 backdrop-blur-lg px-6 py-3 rounded-full">
              <Shield className="w-5 h-5 text-green-400" />
              <span className="text-white font-semibold">Seguro</span>
            </div>
            <div className="flex items-center gap-2 bg-white/10 backdrop-blur-lg px-6 py-3 rounded-full">
              <TrendingUp className="w-5 h-5 text-blue-400" />
              <span className="text-white font-semibold">Escalable</span>
            </div>
          </div>
        </div>
      </section>

      {/* Features */}
      <section id="features" className="py-20 px-4 sm:px-6 lg:px-8 bg-gradient-to-b from-transparent to-slate-800/50">
        <div className="max-w-7xl mx-auto">
          <div className="text-center mb-16">
            <h2 className="text-4xl md:text-5xl font-bold text-white mb-4">
              Características Principales
            </h2>
            <p className="text-xl text-gray-300">
              Todo lo que necesitas para gestionar tu plaza de mercado
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
            {features.map((feature, index) => (
              <div 
                key={index}
                className="bg-white/5 backdrop-blur-lg border border-white/10 rounded-2xl p-6 hover:bg-white/10 hover:scale-105 transition-all duration-300 group"
              >
                <div className="bg-gradient-to-r from-purple-500 to-pink-500 w-16 h-16 rounded-xl flex items-center justify-center mb-4 group-hover:scale-110 transition-transform">
                  {feature.icon}
                </div>
                <h3 className="text-xl font-bold text-white mb-2">{feature.title}</h3>
                <p className="text-gray-300">{feature.description}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Pricing */}
      <section id="pricing" className="py-20 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto">
          <div className="text-center mb-16">
            <h2 className="text-4xl md:text-5xl font-bold text-white mb-4">
              Planes y Precios
            </h2>
            <p className="text-xl text-gray-300">
              Elige el plan perfecto para tu plaza de mercado
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            {plans.map((plan) => (
              <div 
                key={plan.id}
                className={`relative bg-white/5 backdrop-blur-lg border-2 ${plan.popular ? 'border-purple-500 scale-105' : 'border-white/10'} rounded-2xl p-8 hover:scale-105 transition-all duration-300`}
              >
                {plan.popular && (
                  <div className="absolute -top-4 left-1/2 transform -translate-x-1/2">
                    <span className="bg-gradient-to-r from-purple-500 to-pink-500 text-white px-4 py-1 rounded-full text-sm font-bold">
                      Más Popular
                    </span>
                  </div>
                )}
                
                <div className={`bg-gradient-to-r ${plan.color} w-16 h-16 rounded-xl flex items-center justify-center mb-4`}>
                  <Package className="w-8 h-8 text-white" />
                </div>
                
                <h3 className="text-2xl font-bold text-white mb-2">{plan.name}</h3>
                <div className="mb-6">
                  <span className="text-5xl font-bold text-white">{plan.price}</span>
                  <span className="text-gray-400">{plan.period}</span>
                </div>
                
                <ul className="space-y-3 mb-8">
                  {plan.features.map((feature, index) => (
                    <li key={index} className="flex items-start gap-2">
                      <CheckCircle className="w-5 h-5 text-green-400 flex-shrink-0 mt-0.5" />
                      <span className="text-gray-300">{feature}</span>
                    </li>
                  ))}
                </ul>
                
                <button 
                  onClick={() => openModal(plan.id)}
                  className={`w-full bg-gradient-to-r ${plan.color} text-white px-6 py-3 rounded-lg font-bold hover:shadow-2xl hover:scale-105 transition-all`}
                >
                  Solicitar {plan.name}
                </button>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Benefits */}
      <section id="benefits" className="py-20 px-4 sm:px-6 lg:px-8 bg-gradient-to-b from-slate-800/50 to-transparent">
        <div className="max-w-7xl mx-auto">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-12 items-center">
            <div>
              <h2 className="text-4xl md:text-5xl font-bold text-white mb-6">
                Beneficios que Transforman tu Operación
              </h2>
              <p className="text-xl text-gray-300 mb-8">
                PlazaNet no solo es un software, es tu aliado estratégico para crecer y modernizarte.
              </p>
              <div className="space-y-4">
                {benefits.map((benefit, index) => (
                  <div key={index} className="flex items-center gap-3">
                    <CheckCircle className="w-6 h-6 text-green-400 flex-shrink-0" />
                    <span className="text-gray-200 text-lg">{benefit}</span>
                  </div>
                ))}
              </div>
            </div>
            <div className="relative">
              <div className="bg-gradient-to-br from-purple-500/20 to-pink-500/20 backdrop-blur-xl rounded-3xl p-8 border border-white/20">
                <div className="space-y-6">
                  <div className="bg-white/5 rounded-xl p-6">
                    <div className="text-4xl font-bold text-white mb-2">+500</div>
                    <div className="text-gray-300">Plazas Gestionadas</div>
                  </div>
                  <div className="bg-white/5 rounded-xl p-6">
                    <div className="text-4xl font-bold text-white mb-2">98%</div>
                    <div className="text-gray-300">Satisfacción Cliente</div>
                  </div>
                  <div className="bg-white/5 rounded-xl p-6">
                    <div className="text-4xl font-bold text-white mb-2">24/7</div>
                    <div className="text-gray-300">Soporte Técnico</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Testimonials */}
      <section id="testimonials" className="py-20 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto">
          <div className="text-center mb-16">
            <h2 className="text-4xl md:text-5xl font-bold text-white mb-4">
              Lo que Dicen Nuestros Clientes
            </h2>
            <p className="text-xl text-gray-300">
              Historias reales de éxito con PlazaNet
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            {testimonials.map((testimonial, index) => (
              <div 
                key={index}
                className="bg-white/5 backdrop-blur-lg border border-white/10 rounded-2xl p-6 hover:bg-white/10 transition-all"
              >
                <div className="text-yellow-400 text-2xl mb-4">★★★★★</div>
                <p className="text-gray-300 mb-6 italic">"{testimonial.text}"</p>
                <div>
                  <div className="font-bold text-white">{testimonial.name}</div>
                  <div className="text-purple-400 text-sm">{testimonial.role}</div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* CTA */}
      <section className="py-20 px-4 sm:px-6 lg:px-8">
        <div className="max-w-4xl mx-auto text-center">
          <div className="bg-gradient-to-r from-purple-600 to-pink-600 rounded-3xl p-12 shadow-2xl">
            <h2 className="text-4xl md:text-5xl font-bold text-white mb-6">
              ¿Listo para Transformar tu Plaza?
            </h2>
            <p className="text-xl text-white/90 mb-8">
              Únete a cientos de plazas que ya confían en PlazaNet para su gestión diaria
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <button 
                onClick={() => document.getElementById('pricing').scrollIntoView({ behavior: 'smooth' })}
                className="bg-white text-purple-600 px-10 py-4 rounded-full font-bold text-lg hover:shadow-2xl hover:scale-105 transition-all"
              >
                Ver Planes y Precios
              </button>
              <button 
                onClick={onShowLogin}
                className="border-2 border-white text-white px-10 py-4 rounded-full font-bold text-lg hover:bg-white/10 transition-all"
              >
                Acceder al Sistema
              </button>
            </div>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="bg-slate-900/50 border-t border-white/10 py-12 px-4 sm:px-6 lg:px-8">
        <div className="max-w-7xl mx-auto">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8 mb-8">
            <div>
              <div className="flex items-center space-x-2 mb-4">
                <ShoppingCart className="w-6 h-6 text-purple-400" />
                <span className="text-xl font-bold text-white">PlazaNet</span>
              </div>
              <p className="text-gray-400">
                La solución integral para la gestión operativa de plazas de mercado.
              </p>
            </div>
            <div>
              <h4 className="font-bold text-white mb-4">Producto</h4>
              <ul className="space-y-2 text-gray-400">
                <li><a href="#features" className="hover:text-white transition">Características</a></li>
                <li><a href="#pricing" className="hover:text-white transition">Precios</a></li>
                <li><a href="#" className="hover:text-white transition">Casos de Uso</a></li>
              </ul>
            </div>
            <div>
              <h4 className="font-bold text-white mb-4">Empresa</h4>
              <ul className="space-y-2 text-gray-400">
                <li><a href="#" className="hover:text-white transition">Sobre Nosotros</a></li>
                <li><a href="#" className="hover:text-white transition">Blog</a></li>
                <li><a href="#" className="hover:text-white transition">Contacto</a></li>
              </ul>
            </div>
            <div>
              <h4 className="font-bold text-white mb-4">Soporte</h4>
              <ul className="space-y-2 text-gray-400">
                <li><a href="#" className="hover:text-white transition">Centro de Ayuda</a></li>
                <li><a href="#" className="hover:text-white transition">Documentación</a></li>
                <li><a href="#" className="hover:text-white transition">Estado del Sistema</a></li>
              </ul>
            </div>
          </div>
          <div className="border-t border-white/10 pt-8 text-center text-gray-400">
            <p>&copy; 2025 PlazaNet. Todos los derechos reservados.</p>
          </div>
        </div>
      </footer>
    </div>
  );
}
