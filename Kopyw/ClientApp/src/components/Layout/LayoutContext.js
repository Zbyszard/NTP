import React from 'react';

const LayoutContext = React.createContext({
    messageViewEnabled: false,
    setMessageViewEnabled: value => {}
});

export default LayoutContext;