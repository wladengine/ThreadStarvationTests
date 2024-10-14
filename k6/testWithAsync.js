import http from 'k6/http';

export const options = {
  // A number specifying the number of VUs to run concurrently.
  vus: 100,
  // A string specifying the total duration of the test run.
  duration: '60s',
};

export default function() {
  http.get('https://localhost:7085/products/async');
}
