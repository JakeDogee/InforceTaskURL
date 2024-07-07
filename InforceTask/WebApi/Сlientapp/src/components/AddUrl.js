import React, { useState } from 'react';
import axios from 'axios';

const AddUrl = ({ onAddUrl }) => {
    const [url, setUrl] = useState('');
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        try {
            const response = await axios.post('/api/url', url, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            onAddUrl(response.data);
            setUrl('');
        } catch (error) {
            setError(error.response?.data || 'Error: Could not shorten URL.');
        }
    };

    return (
        <div>
            <h2>Add New URL</h2>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    value={url}
                    onChange={(e) => setUrl(e.target.value)}
                    placeholder="Enter URL"
                    required
                />
                <button type="submit">Shorten</button>
            </form>
        </div>
    );
};

export default AddUrl;
