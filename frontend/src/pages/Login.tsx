import '../styles/login.css'
import { getSpotifyLoginUrl } from '../api/spotify'

function Login() {
  async function handleLogin() {
    try {
      const url = await getSpotifyLoginUrl();
      window.location.href = url;
    } catch (err) {
      console.error("Failed to start Spotify login:", err);
    }
  };

  return (
    <main className="app-container">
      <div className="card login-card">
        <button onClick={handleLogin} className="primary-button">
          Login with Spotify
        </button>
      </div>
    </main>
  );
}

export default Login;