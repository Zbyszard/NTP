import React from 'react';

const AuthContext = React.createContext({
    authorized: false,
    userName: undefined
});

export default AuthContext;