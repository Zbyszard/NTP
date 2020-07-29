import React from 'react';

const MenuContext = React.createContext({
    navItemClicked: () => { },
    enableSearch: () => { },
    disableSearch: () => { },
    searchIconClick: () => { },
    showSearch: false,
    searchInputRef: null
});

export default MenuContext;