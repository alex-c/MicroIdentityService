function processResponse(response) {
  return new Promise((resolve, reject) => {
    if (response.status === 204) {
      resolve({ status: response.status });
    } else if (response.status === 401 || response.status === 403) {
      reject({ status: response.status });
    } else {
      let handler;
      response.status < 400 ? (handler = resolve) : (handler = reject);
      response.json().then(json => handler({ status: response.status, body: json }));
    }
  });
}

function catchNetworkError(response) {
  return new Promise((_, reject) => {
    reject({ status: null, message: 'general.networkError' });
  });
}

function getAuthorizationHeader() {
  return 'Bearer ' + localStorage.getItem('token');
}

const SERVER_ENDPOINT = process.env.VUE_APP_SERVER_ENDPOINT;

export default {
  identities: {
    getIdentities: (page, elementsPerPage) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/identities?page=${page}&elementsPerPage=${elementsPerPage}`, {
        method: 'GET',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    updateIdentity: (id, disabled) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/identities/${id}`, {
        method: 'PUT',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
        body: JSON.stringify({ disabled }),
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    deleteIdentity: id => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/identities/${id}`, {
        method: 'DELETE',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
  },
  domains: {
    getDomains: (filter, page, elementsPerPage) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/domains?filter=${filter}&page=${page}&elementsPerPage=${elementsPerPage}`, {
        method: 'GET',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    createDomain: name => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/domains`, {
        method: 'POST',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
        body: JSON.stringify({ name }),
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
  },
  roles: {
    getRoles: (page, elementsPerPage) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/roles?page=${page}&elementsPerPage=${elementsPerPage}`, {
        method: 'GET',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    createRole: (name, domainId) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/roles`, {
        method: 'POST',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
        body: JSON.stringify({ name, domainId }),
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
  },
};
