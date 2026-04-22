//npm install @microsoft/signalr

import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

export default function PaymentStatus() {
    const [paymentRequestId, setPaymentRequestId] = useState("");
    const [status, setStatus] = useState("Not started");
    const [message, setMessage] = useState("");
    const [connection, setConnection] = useState(null);

    useEffect(() => {
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5000/hubs/payment-status")
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);

        return () => {
            if (newConnection.state === signalR.HubConnectionState.Connected) {
                newConnection.stop();
            }
        };
    }, []);

    useEffect(() => {
        if (!connection) return;

        connection.on("PaymentStatusUpdated", (payload) => {
            setStatus(payload.status);
            setMessage(payload.message || "");
        });

        connection
            .start()
            .then(() => console.log("SignalR connected"))
            .catch((err) => console.error("SignalR connection error:", err));

        return () => {
            connection.off("PaymentStatusUpdated");
        };
    }, [connection]);

    const createPayment = async () => {
        const response = await fetch("http://localhost:5000/api/payments", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                amount: 100,
                currency: "USD"
            })
        });

        const data = await response.json();

        setPaymentRequestId(data.paymentRequestId);
        setStatus(data.status);
        setMessage("Waiting for webhook...");

        if (connection && connection.state === signalR.HubConnectionState.Connected) {
            await connection.invoke("SubscribeToPayment", data.paymentRequestId);
        }
    };

    const simulateWebhookSuccess = async () => {
        if (!paymentRequestId) return;

        await fetch("http://localhost:5000/api/payments/webhook", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                paymentRequestId,
                success: true
            })
        });
    };

    const simulateWebhookFail = async () => {
        if (!paymentRequestId) return;

        await fetch("http://localhost:5000/api/payments/webhook", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                paymentRequestId,
                success: false
            })
        });
    };

    return (
        <div style={{ padding: "24px", fontFamily: "Arial" }}>
            <h2>Payment Status Demo</h2>

            <button onClick={createPayment}>Create Payment</button>
            <button onClick={simulateWebhookSuccess} disabled={!paymentRequestId} style={{ marginLeft: "8px" }}>
                Simulate Success Webhook
            </button>
            <button onClick={simulateWebhookFail} disabled={!paymentRequestId} style={{ marginLeft: "8px" }}>
                Simulate Fail Webhook
            </button>

            <div style={{ marginTop: "20px" }}>
                <p><strong>PaymentRequestId:</strong> {paymentRequestId || "-"}</p>
                <p><strong>Status:</strong> {status}</p>
                <p><strong>Message:</strong> {message}</p>
            </div>
        </div>
    );
}
