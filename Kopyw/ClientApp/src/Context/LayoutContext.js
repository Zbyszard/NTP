import React, { useContext } from 'react';

const LayoutContext = React.createContext({
    messageViewEnabled: false,
    setMessageViewEnabled: value => { }
});

export const withLayoutContext = Component => {
    return props => {
        const layoutContext = useContext(LayoutContext);
        return <Component {...props} layoutContext={layoutContext} />;
    }
}

export default LayoutContext;