import React, { useState, useEffect } from 'react';

function Teams() {
  const [teams, setTeams] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    const codespaceName = process.env.REACT_APP_CODESPACE_NAME;
    const endpoint = codespaceName
      ? `https://${codespaceName}-8000.app.github.dev/api/teams/`
      : 'http://localhost:8000/api/teams/';

    console.log('Fetching teams from:', endpoint);

    fetch(endpoint)
      .then(res => res.json())
      .then(data => {
        console.log('Fetched teams data:', data);
        const items = data.results ? data.results : data;
        setTeams(items);
      })
      .catch(err => {
        console.error('Error fetching teams:', err);
        setError(err.message);
      });
  }, []);

  return (
    <div>
      <h2 className="mb-4">Teams</h2>
      {error && <div className="alert alert-danger">Error: {error}</div>}
      <table className="table table-striped table-hover">
        <thead className="table-dark">
          <tr>
            <th>Team Name</th>
            <th>Members</th>
          </tr>
        </thead>
        <tbody>
          {teams.map((team, index) => (
            <tr key={team.id || index}>
              <td>{team.name}</td>
              <td>
                {Array.isArray(team.members)
                  ? team.members.join(', ')
                  : team.members}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {teams.length === 0 && !error && (
        <p className="text-muted">Loading teams...</p>
      )}
    </div>
  );
}

export default Teams;
