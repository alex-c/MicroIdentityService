import store from '../store/store.js';

// Redirects a signed-in administrator to the private page
function redirectAuthorizedUser(_to, _from, next) {
  if (store.state.user.token !== null) {
    next({ path: '/private' });
  } else {
    next();
  }
}

// Redirects an unauthorized user to the public page
function redirectUnauthorizedUser(_to, _from, next) {
  if (store.state.user.token === null) {
    next({ path: '/' });
  } else {
    next();
  }
}

export { redirectAuthorizedUser, redirectUnauthorizedUser };
