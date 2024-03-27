import React, { useState, useEffect } from 'react';
import Form from 'react-bootstrap/Form';
import Table from 'react-bootstrap/Table';

import axios from 'axios';

const Search = () => {
    const [data, setData] = useState([]);

    const [url, setUrl] = useState('');
    const [keywords, setKeywords] = useState('');

    useEffect(() => {
        getData();
    }, []);

    const getData = () => {
        axios.get('http://localhost:44472/api/Search')
            .then((result) => { setData(result.data); })
            .catch((error) => { console.log(error); });
    };

    const handleSearch = () => {
        axios.post('http://localhost:44472/api/Search', {
            'url': url,
            'keywords': keywords
        })
            .then((result) => {
                getData();
                clear();
            })
            .catch((error) => { console.log(error); });

        clear();
    }

    const clear = () => {
        setUrl('');
        setKeywords('');
    }

    return (
        <React.Fragment>
            <React.Fragment>
                <Form className="mb-3">
                    <Form.Group className="mb-3" controlId="form.url">
                        <Form.Label>URL</Form.Label>
                        <Form.Control type="text" value={url} onChange={(e) => setUrl(e.target.value)} />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="form.keywords">
                        <Form.Label>Kewyords</Form.Label>
                        <Form.Control type="text" value={keywords} onChange={(e) => setKeywords(e.target.value)} />
                    </Form.Group>
                    <Form.Group className="mb-3">
                        <button className="btn btn-success" onClick={() => handleSearch()}>Search</button>
                    </Form.Group>
                </Form>
            </React.Fragment>
            <React.Fragment>
                <Table striped bordered hover>
                    <thead>
                        <tr>
                            <th>URL</th>
                            <th>Keywords</th>
                            <th>Indexes</th>
                            <th>Created On</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            data && data.length > 0
                                ? (data.map((item, index) => {
                                    return (
                                        <tr key={index}>
                                            <td>{item.url}</td>
                                            <td>{item.keywords}</td>
                                            <td>{
                                                item.indexes && item.indexes.length > 0
                                                    ? item.indexes.join(',')
                                                    : ''
                                            }</td>
                                            <td>{item.createdOnUtc}</td>
                                        </tr>
                                    )
                                }))
                                : (
                                    <tr>
                                        <td colSpan="4">No data available</td>
                                    </tr>
                                )
                        }
                    </tbody>
                </Table>
            </React.Fragment>
        </React.Fragment>
    );
}

export default Search;