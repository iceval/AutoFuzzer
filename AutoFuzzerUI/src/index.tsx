import { StrictMode } from 'react';
import ReactDOM from 'react-dom/client';

import App from './App';

import './styles.scss';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <StrictMode>
    <App />
  </StrictMode>
);
