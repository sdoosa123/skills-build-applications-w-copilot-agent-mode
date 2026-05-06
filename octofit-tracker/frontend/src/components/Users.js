import React, { useState, useEffect } from 'react';

function Users() {
  const [users, setUsers] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    const codespaceName = process.env.REACT_APP_CODESPACE_NAME;
    const endpoint = codespaceName
      ? `https://${codespaceName}-8000.app.github.dev/api/users/`
      : 'http://localhost:8000/api/users/';

    console.log('Fetching users from:', endpoint);

    fetch(endpoint)
      .then(res => res.json())
      .then(data => {
        console.log('Fetched users data:', data);
        const items = data.results ? data.results : data;
        setUsers(items);
      })
      .catch(err => {
        console.error('Error fetching users:', err);
        setError(err.message);
      });
  }, []);

  return (
    <div>
      <h2 className="mb-4">Users</h2>
      {error && <div className="alert alert-danger">Error: {error}</div>}
      <table className="table table-striped table-hover">
        <thead className="table-dark">
          <tr>
            <th>Username</th>
            <th>Email</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user, index) => (
            <tr key={user.id || index}>
              <td>{user.username}</td>
              <td>{user.email}</td>
            </tr>
          ))}
        </tbody>
      </table>
      {users.length === 0 && !error && (
        <p className="text-muted">Loading users...</p>
      )}
    </div>
  );
}

export default Users;
