import { useState } from "react";
import "../styles/dashboard.css";
import { getSpotifyStats, type Artist, type Song } from "../api/spotify";
import SpotifyDumb from "../components/SpotifyDumb";

function Dashboard() {
  const [topTracks, setTopTracks] = useState<Song[]>([]);
  const [topArtists, setTopArtists] = useState<Artist[]>([]);
  const [loading, setLoading] = useState(false);
  const [isSpotifyDumb, setIsSpotifyDumb] = useState<boolean>(false);

  async function loadSpotifyData() {
    setLoading(true);
    setIsSpotifyDumb(false);

    const token = JSON.parse(localStorage.getItem("spotify_token") || "null");
    if (!token) {
      setLoading(false);
      return;
    }

    try {
      const { tracks, artists } = await getSpotifyStats(token);
      setTopTracks(tracks);
      setTopArtists(artists);
    } catch (err: any) {
      if (err.message === "403") {
        setIsSpotifyDumb(true);
      }
    } finally {
      setLoading(false);
    }
  }

  return (
    <main className="app-container">
      <div className="card dashboard-card">
        <header className="dashboard-header">
          <h1>Your Spotify Stats</h1>
          <button
            onClick={loadSpotifyData}
            disabled={loading}
            className="primary-button"
          >
            {loading ? "Loading..." : "Load Stats"}
          </button>
        </header>

        <div className="dashboard-content">
          <section className="top-section">
            <h2>Top Artists</h2>
            <div className="artists-grid">
              {topArtists.map((artist, i) => (
                <div key={i} className="artist-card">
                  <img src={artist.imageUrl} alt={artist.name} />
                  <span>{artist.name}</span>
                </div>
              ))}
            </div>
          </section>

          <section className="top-section">
            <h2>Top Tracks</h2>
            <div className="tracks-list">
              {topTracks.map((track, i) => (
                <div key={i} className="track-item">
                  <span className="track-number">{i + 1}.</span>
                  <div className="track-info">
                    <strong>{track.title}</strong>
                    <span> - </span>
                    <span>{track.artists.join(", ")}</span>
                  </div>
                </div>
              ))}
            </div>
          </section>

          {isSpotifyDumb && <SpotifyDumb/>}

        </div>
      </div>
    </main>
  );
}

export default Dashboard;