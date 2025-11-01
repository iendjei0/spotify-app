import '../styles/login.css'

function Login() {
  const handleLogin = () => {
    window.location.href = `${BACKEND_URL}/spotify/login`;
  }
  
  return (
    <main className="app-container">
      <div className="card login-card">
        <button onClick={handleLogin} className="primary-button">
          Login with Spotify
        </button>
      </div>
    </main>
  )
}

export default Login;
