import React, { useState } from 'react';
import AddUrl from './AddUrl';
import UrlTable from './UrlTable';

const UrlShortener = () => {
    const [urls, setUrls] = useState([]);

    const handleAddUrl = (newUrl) => {
        setUrls([...urls, newUrl]);
    };

    return (
        <div>
            <h1>URL Shortener</h1>
            <AddUrl onAddUrl={handleAddUrl} />
            <UrlTable urls={urls} />
        </div>
    );
};

export default UrlShortener;
