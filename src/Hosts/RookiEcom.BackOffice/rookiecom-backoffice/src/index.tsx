import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './containers/App.tsx';
import './index.css';
import { AuthProvider } from 'react-oidc-context';
import oidcConfig from './oidc/oidcConfig.ts';
import { Provider } from 'react-redux';
import store from './store/store.ts';

// import 'bootstrap/dist/css/bootstrap.min.css';
// import '@mdbootstrap/react/dist/css/mdb.min.css';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    {/* <Provider store={store}> */}
    <AuthProvider {...oidcConfig}>
      <App />
    </AuthProvider>
    {/* </Provider> */}
  </React.StrictMode>
);
