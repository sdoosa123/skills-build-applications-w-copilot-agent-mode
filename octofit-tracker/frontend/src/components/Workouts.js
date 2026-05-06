import React, { useState, useEffect } from 'react';

function Workouts() {
  const [workouts, setWorkouts] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    const codespaceName = process.env.REACT_APP_CODESPACE_NAME;
    const endpoint = codespaceName
      ? `https://${codespaceName}-8000.app.github.dev/api/workouts/`
      : 'http://localhost:8000/api/workouts/';

    console.log('Fetching workouts from:', endpoint);

    fetch(endpoint)
      .then(res => res.json())
      .then(data => {
        console.log('Fetched workouts data:', data);
        const items = data.results ? data.results : data;
        setWorkouts(items);
      })
      .catch(err => {
        console.error('Error fetching workouts:', err);
        setError(err.message);
      });
  }, []);

  return (
    <div>
      <h2 className="mb-4">Workouts</h2>
      {error && <div className="alert alert-danger">Error: {error}</div>}
      <table className="table table-striped table-hover">
        <thead className="table-dark">
          <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Duration (mins)</th>
          </tr>
        </thead>
        <tbody>
          {workouts.map((workout, index) => (
            <tr key={workout.id || index}>
              <td>{workout.name}</td>
              <td>{workout.description}</td>
              <td>{workout.duration}</td>
            </tr>
          ))}
        </tbody>
      </table>
      {workouts.length === 0 && !error && (
        <p className="text-muted">Loading workouts...</p>
      )}
    </div>
  );
}

export default Workouts;
