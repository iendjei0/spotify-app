import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Login from './pages/Login';
import SpotifyCallback from './pages/SpotifyCallback';
import Dashboard from './pages/Dashboard';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login/>}/>
        <Route path="/callback" element={<SpotifyCallback/>}/>
        <Route path="/dashboard" element={<Dashboard/>}/>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
