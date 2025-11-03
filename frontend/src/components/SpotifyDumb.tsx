import "../styles/dashboard.css";

function SpotifyDumb() {
  return (
    <div className="spotify-warning">
      <p>
        Due to Spotify's incredibly dumb restrictions on apps in development mode, 
        only users I manually invite via email can actually log in.  
        <br />
        Btw sometimes it just randomly works for uninvited users, which is both hilarious and sad.
      </p>
      <img src="/spotifydumb.png" className="spotify-dumb-img" alt="Spotify dev mode restrictions" />
    </div>
  );
}

export default SpotifyDumb;