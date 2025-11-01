import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { exchangeSpotifyCode } from "../api/spotify";

function SpotifyCallback() {
  const navigate = useNavigate();

  useEffect(() => {
    const params = new URLSearchParams(window.location.search);
    const code = params.get("code");

    if (!code) {
      console.error("No code in callback");
      return;
    }

    exchangeSpotifyCode(code)
      .then((data) => {
        localStorage.setItem("spotify_token", JSON.stringify(data.access_token));
        navigate("/dashboard");
      })
      .catch((err) => {
        console.error("Spotify code exchange failed", err);
      });
  }, [navigate]);

  return <div>Logging in with Spotify...</div>;
}

export default SpotifyCallback;