export const environment = {
    production: false,
    apiUrl: '<<YOUR_BACKEND_API_URL_HERE>>', // e.g., 'https://your-backend-url.com/api'
    Auth0: {
        Domain: '<<YOUR_AUTH0_DOMAIN_HERE>>', // e.g., 'your-tenant.auth0.com'
        Audience: '<<YOUR_AUTH0_AUDIENCE_HERE>>', // e.g., 'https://your-api-identifier'
        ClientId: '<<YOUR_AUTH0_CLIENT_ID_HERE>>', // e.g., 'abc123XYZ'
        ClientSecret: '<<YOUR_AUTH0_CLIENT_SECRET_HERE>>', // Only needed for confidential flows
    },
    firebase: {
        apiKey: '<<YOUR_FIREBASE_API_KEY_HERE>>',
        authDomain: '<<YOUR_FIREBASE_AUTH_DOMAIN_HERE>>', // e.g., 'your-app.firebaseapp.com'
        projectId: '<<YOUR_FIREBASE_PROJECT_ID_HERE>>',
        storageBucket: '<<YOUR_FIREBASE_STORAGE_BUCKET_HERE>>', // e.g., 'your-app.appspot.com'
        messagingSenderId: '<<YOUR_FIREBASE_MESSAGING_SENDER_ID_HERE>>',
        appId: '<<YOUR_FIREBASE_APP_ID_HERE>>',
        measurementId: '<<YOUR_FIREBASE_MEASUREMENT_ID_HERE>>',
    },
};
