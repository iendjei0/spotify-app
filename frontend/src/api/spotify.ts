
export async function getSpotifyLoginUrl(): Promise<string> {
  const res = await fetch(`${BACKEND_URL}/spotify/login`);

  if (!res.ok) {
    throw new Error(`Failed to get Spotify login URL: ${res.statusText}`);
  }

  const data = await res.json();
  if (!data.url) {
    throw new Error("Spotify login URL missing in response");
  }

  return data.url;
}

export interface SpotifyExchangeResponse {
  access_token: string;
  refresh_token?: string;
  expires_in?: number;
}

export async function exchangeSpotifyCode(code: string): Promise<SpotifyExchangeResponse> {
  const res = await fetch(`${BACKEND_URL}/spotify/exchange`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ code }),
  });

  if (!res.ok) {
    throw new Error(`Spotify exchange failed: ${res.statusText}`);
  }

  return res.json();
}

export interface Artist {
  name: string;
  imageUrl: string;
}

export interface Song {
  title: string;
  artists: string[];
}

export async function getTopTracks(token: string): Promise<Song[]> {
  const res = await fetch(`${BACKEND_URL}/spotify/top-tracks`, {
    headers: { Token: token },
  });

  if (res.status === 403) throw new Error("403");
  if (!res.ok) throw new Error("Failed to fetch top tracks");

  return res.json();
}

export async function getTopArtists(token: string): Promise<Artist[]> {
  const res = await fetch(`${BACKEND_URL}/spotify/top-artists`, {
    headers: { Token: token },
  });

  if (res.status === 403) throw new Error("403");
  if (!res.ok) throw new Error("Failed to fetch top artists");

  return res.json();
}

export async function getSpotifyStats(token: string): Promise<{
  tracks: Song[];
  artists: Artist[];
}> {
  const [tracks, artists] = await Promise.all([
    getTopTracks(token),
    getTopArtists(token),
  ]);
  return { tracks, artists };
}