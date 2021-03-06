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
  authenticate: (identifier, password) => {
    return fetch(`${SERVER_ENDPOINT}/api/v1/auth/identity`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ identifier, password }),
    })
      .catch(catchNetworkError)
      .then(processResponse);
  },
  identities: {
    getIdentities: (filter, showDisabled, page, elementsPerPage) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/identities?filter=${filter}&showDisabled=${showDisabled}&page=${page}&elementsPerPage=${elementsPerPage}`, {
        method: 'GET',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    getAll: function() {
      return this.getIdentities('', false, 1, -1);
    },
    getIdentity: id => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/identities/${id}`, {
        method: 'GET',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    createIdentity: (identifier, password) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/identities`, {
        method: 'POST',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
        body: JSON.stringify({ identifier, password }),
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    updateIdentityStatus: (id, disabled) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/identities/${id}/status`, {
        method: 'PUT',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
        body: JSON.stringify({ disabled }),
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    updateIdentityRoles: (id, roleIds) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/identities/${id}/roles`, {
        method: 'PUT',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
        body: JSON.stringify({ roleIds }),
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    deleteIdentity: (id, softDelete) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/identities/${id}?softDelete=${softDelete}`, {
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
    getAll: function() {
      return this.getDomains('', 1, -1);
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
    deleteDomain: id => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/domains/${id}`, {
        method: 'DELETE',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
  },
  roles: {
    getRoles: (filter, page, elementsPerPage, domainId) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/roles?filter=${filter}&page=${page}&elementsPerPage=${elementsPerPage}&domainId=${domainId}`, {
        method: 'GET',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    getAll: function() {
      return this.getRoles('', 1, -1);
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
    deleteRole: id => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/roles/${id}`, {
        method: 'DELETE',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
  },
  apiKeys: {
    getApiKeys: (filter, page, elementsPerPage) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/api-keys?filter=${filter}&page=${page}&elementsPerPage=${elementsPerPage}`, {
        method: 'GET',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    getAll: function() {
      return this.getApiKeys('', 1, -1);
    },
    createApiKey: name => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/api-keys`, {
        method: 'POST',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
        body: JSON.stringify({ name }),
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    updateApiKeyStatus: (id, enabled) => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/api-keys/${id}/status`, {
        method: 'PUT',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
        body: JSON.stringify({ enabled }),
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
    deleteApiKey: id => {
      return fetch(`${SERVER_ENDPOINT}/api/v1/api-keys/${id}`, {
        method: 'DELETE',
        withCredentials: true,
        credentials: 'include',
        headers: { 'Content-Type': 'application/json', Authorization: getAuthorizationHeader() },
      })
        .catch(catchNetworkError)
        .then(processResponse);
    },
  },
};
