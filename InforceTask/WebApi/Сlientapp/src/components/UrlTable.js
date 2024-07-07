import React, { useEffect, useState } from 'react';
import axios from 'axios';

const UrlTable = () => {
    const [urls, setUrls] = useState([]);
    const [newUrl, setNewUrl] = useState('');

    useEffect(() => {
        axios.get('/api/url')
            .then(response => setUrls(response.data))
            .catch(error => console.error(error));
    }, []);

    const handleAddUrl = () => {
        axios.post('/api/url', newUrl)
            .then(response => setUrls([...urls, response.data]))
            .catch(error => console.error(error));
    };

    const handleDeleteUrl = (id) => {
        axios.delete(`/api/url/${id}`)
            .then(() => setUrls(urls.filter(url => url.id !== id)))
            .catch(error => console.error(error));
    };

    return (
        <div>
            <table>
                <thead>
                <tr>
                    <th>Original URL</th>
                    <th>Short URL</th>
                    <th>Actions</th>
                </tr>
                </thead>
                <tbody>
                {urls.map(url => (
                    <tr key={url.id}>
                        <td>{url.originalUrl}</td>
                        <td>{url.shortUrl}</td>
                        <td>
                            <button onClick={() => handleDeleteUrl(url.id)}>Delete</button>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
            <input
                type="text"
                value={newUrl}
                onChange={e => setNewUrl(e.target.value)}
                placeholder="Enter URL"
            />
            <button onClick={handleAddUrl}>Add URL</button>
        </div>
    );
};

export default UrlTable;
