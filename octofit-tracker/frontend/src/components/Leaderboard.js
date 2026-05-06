import React, { useState, useEffect } from 'react';

function Leaderboard() {
  const [entries, setEntries] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    const codespaceName = process.env.REACT_APP_CODESPACE_NAME;
    const endpoint = codespaceName
      ? `https://${codespaceName}-8000.app.github.dev/api/leaderboard/`
      : 'http://localhost:8000/api/leaderboard/';

    console.log('Fetching leaderboard from:', endpoint);

    fetch(endpoint)
      .then(res => res.json())
      .then(data => {
        console.log('Fetched leaderboard data:', data);
        const items = data.results ? data.results : data;
        setEntries(items);
      })
      .catch(err => {
        console.error('Error fetching leaderboard:', err);
        setError(err.message);
      });
  }, []);

  return (
    <div>
      <h2 className="mb-4">Leaderboard</h2>
      {error && <div className="alert alert-danger">Error: {error}</div>}
      <table className="table table-striped table-hover">
        <thead className="table-dark">
          <tr>
            <th>Rank</th>
            <th>User</th>
            <th>Score</th>
          </tr>
        </thead>
        <tbody>
          {entries.map((entry, index) => (
            <tr key={entry.id || index}>
              <td>{index + 1}</td>
              <td>{entry.user ? entry.user.username : 'Unknown'}</td>
              <td>{entry.score}</td>
            </tr>
          ))}
        </tbody>
      </table>
      {entries.length === 0 && !error && (
        <p className="text-muted">Loading leaderboard...</p>
      )}
    </div>
  );
}

export default Leaderboard;
