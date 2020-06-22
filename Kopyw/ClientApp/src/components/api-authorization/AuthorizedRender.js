import { useContext } from 'react';
import AuthContext from './AuthContext';

const AuthorizedRender = props => {
    const authContext = useContext(AuthContext);
    if (authContext.authorized)
        return props.children;
    return null;
}

export default AuthorizedRender;