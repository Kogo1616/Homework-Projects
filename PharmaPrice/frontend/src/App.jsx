import { useEffect, useState } from 'react'
import './App.css'

// dev-ში backend ცალკე პორტზეა; production-ში იმავე სერვერიდან იდება (relative).
const API = import.meta.env.DEV ? 'http://localhost:5057' : ''

const PHARMACY_COLORS = {
  PSP: '#e2001a',
  GPC: '#0a7d34',
  ფარმადეპო: '#1a4f9c',
}

function App() {
  const [search, setSearch] = useState('')
  const [results, setResults] = useState([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')
  const [searched, setSearched] = useState(false)

  useEffect(() => {
    if (search.trim().length < 2) {
      setResults([])
      setSearched(false)
      return
    }

    const controller = new AbortController()
    const timer = setTimeout(async () => {
      setLoading(true)
      setError('')
      try {
        const url = `${API}/api/search?q=${encodeURIComponent(search.trim())}`
        const res = await fetch(url, { signal: controller.signal })
        if (!res.ok) throw new Error('სერვერის შეცდომა')
        setResults(await res.json())
        setSearched(true)
      } catch (e) {
        if (e.name !== 'AbortError') setError('ვერ დავუკავშირდი სერვერს. გაშვებულია backend?')
      } finally {
        setLoading(false)
      }
    }, 400)

    return () => {
      clearTimeout(timer)
      controller.abort()
    }
  }, [search])

  return (
    <div className="app">
      <header className="header">
        <h1>💊 PharmaPrice</h1>
        <p>რეალური ფასები: PSP · GPC · ფარმადეპო</p>
      </header>

      <input
        className="search"
        type="text"
        placeholder="ჩაწერე წამლის სახელი (მაგ. მიგ 400)..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        autoFocus
      />

      {loading && <p className="muted">ვეძებ ყველა აფთიაქში...</p>}
      {error && <p className="error">{error}</p>}
      {!loading && !error && searched && results.length === 0 && (
        <p className="muted">ვერაფერი მოიძებნა.</p>
      )}
      {!loading && searched && results.length > 0 && (
        <p className="count">ნაპოვნია {results.length} წამალი — დალაგებულია იაფიდან</p>
      )}

      <div className="results">
        {results.map((m, i) => (
          <MedicineCard key={`${m.name}-${i}`} medicine={m} />
        ))}
      </div>
    </div>
  )
}

function MedicineCard({ medicine }) {
  return (
    <div className="card">
      <div className="card-head">
        {medicine.imageUrl ? (
          <img className="thumb" src={medicine.imageUrl} alt="" loading="lazy" />
        ) : (
          <div className="thumb placeholder">💊</div>
        )}
        <span className="mname">{medicine.name}</span>
      </div>

      <ul className="offers">
        {medicine.offers.map((o) => {
          const color = PHARMACY_COLORS[o.pharmacy] || '#555'
          return (
            <li key={o.pharmacy}>
              <a
                className={o.isCheapest ? 'offer cheapest' : 'offer'}
                href={o.url}
                target="_blank"
                rel="noreferrer"
              >
                <span className="badge" style={{ background: color }}>{o.pharmacy}</span>
                <span className="spacer" />
                {o.isCheapest && <span className="cheap-tag">ყველაზე იაფი</span>}
                {o.oldPrice && <span className="old">{o.oldPrice.toFixed(2)} ₾</span>}
                <span className="price">{o.price.toFixed(2)} ₾</span>
              </a>
            </li>
          )
        })}
      </ul>
    </div>
  )
}

export default App
