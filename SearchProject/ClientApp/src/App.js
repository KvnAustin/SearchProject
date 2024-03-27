import React, { Component } from 'react';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Search from './components/Search';

export default class App extends Component {
    render() {
        return (
            <React.Fragment>
                <Container>
                    <Row>
                        <Col>
                            <h1>G-Scraper</h1>
                            <Search />
                        </Col>
                    </Row>
                </Container>
            </React.Fragment>
        );
    }
}
