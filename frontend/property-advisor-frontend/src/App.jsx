import { useEffect, useState } from "react";

export default function App() {
  const [data, setData] = useState(null);
  const [err, setErr] = useState(null);

  useEffect(() => {
    (async () => {
      try {
        // try a simple, always-available endpoint first
        const res = await fetch("/api/health");
        const text = await res.text();
        setData({ health: text });

        // then try listings (adjust if your route differs)
        const r2 = await fetch("/api/v1/listings?page=1&pageSize=5");
        if (!r2.ok) throw new Error(`Listings HTTP ${r2.status}`);
        const json = await r2.json();
        setData((d) => ({ ...d, listings: json }));
      } catch (e) {
        console.error(e);
        setErr(String(e));
      }
    })();
  }, []);

  return (
    <div style={{ color: "#eee", padding: "2rem", fontFamily: "system-ui, sans-serif" }}>
      <h1>Frontend ↔ API check</h1>
      {err && (
        <p style={{ color: "#ff8c8c" }}>
          Error: {err} — is the API running at <code>https://localhost:7130</code>?
          Open Swagger once to trust the HTTPS certificate.
        </p>
      )}
      <pre style={{ background: "#222", padding: "1rem", overflowX: "auto" }}>
        {JSON.stringify(data, null, 2)}
      </pre>
    </div>
  );
}
