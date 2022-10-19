import ReactDOM from 'react-dom/client';
import "semantic-ui-css/semantic.min.css";
import App from "./app/layout/App";
import { BrowserRouter as Router } from "react-router-dom";
import { store, StoreContext } from './app/stores/store';
import reportWebVitals from './reportWebVitals';
import { Container } from 'semantic-ui-react';

const container = document.getElementById("root");
const root = ReactDOM.createRoot(container as HTMLElement);

root.render(
  <StoreContext.Provider value={store}>
    <Container>
      <Router>
        <App />
      </Router>
    </Container>
  </StoreContext.Provider>
);

reportWebVitals();