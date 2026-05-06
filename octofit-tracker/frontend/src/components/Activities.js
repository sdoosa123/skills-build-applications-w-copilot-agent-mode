import React, { useState, useEffect } from 'react';

function Activities() {
  const [activities, setActivities] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    const codespaceName = process.env.REACT_APP_CODESPACE_NAME;
    const endpoint = codespaceName
      ? `https://${codespaceName}-8000.app.github.dev/api/activities/`
      : 'http://localhost:8000/api/activities/';

    console.log('Fetching activities from:', endpoint);

    fetch(endpoint)
      .then(res => res.json())
      .then(data => {
        console.log('Fetched activities data:', data);
        const items = data.results ? data.results : data;
        setActivities(items);
      })
      .catch(err => {
        console.error('Error fetching activities:', err);
        setError(err.message);
      });
  }, []);

  return (
    <div>
      <h2 className="mb-4">Activities</h2>
      {error && <div className="alert alert-danger">Error: {error}</div>}
      <table className="table table-striped table-hover">
        <thead className="table-dark">
          <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Schedule</th>
            <th>Max Participants</th>
          </tr>
        </thead>
        <tbody>
          {activities.map((activity, index) => (
            <tr key={activity.id || index}>
              <td>{activity.name}</td>
              <td>{activity.description}</td>
              <td>{activity.schedule}</td>
              <td>{activity.max_participants}</td>
            </tr>
          ))}
        </tbody>
      </table>
      {activities.length === 0 && !error && (
        <p className="text-muted">Loading activities...</p>
      )}
    </div>
  );
}

export default Activities;
