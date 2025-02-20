import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import RegistrationPage from './Components/RegistrationPage';
import LoginPage from './Components/LoginPage';
import SolarWatchPage from './Components/SolarWatchPage';

function ProtectedRoute({ children }) {
  const token = localStorage.getItem("token");
  return token ? children : <Navigate to="/login" />;
}

const App = () => {
  return (
    <Router>
      <Routes>
        <Route path="/register" element={<RegistrationPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path='/solar-watch' element={
          <ProtectedRoute>
            <SolarWatchPage />
          </ProtectedRoute>
          }
        />
      </Routes>
    </Router>
  );
};

export default App;