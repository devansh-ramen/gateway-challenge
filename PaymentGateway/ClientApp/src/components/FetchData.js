import React, { Component } from 'react';

export class FetchData extends Component {
    static displayName = FetchData.name;
    merchantId = "";

    constructor({ match, ...props }) {
        super(props);

        let search = window.location.search;
        let params = new URLSearchParams(search);
        this.merchantId = params.get('merchantId');

        this.state = {
            paymentsResult: [],
            loading: true,
        };


    }

    componentDidMount() {
        this.fetchPaymentsForMerchant(this.merchantId);
    }
    

    static renderPaymentsTable(paymentList) {
        
        return (
            <form onSubmit={this.handleSubmit}>
                <label>
                    Enter your MerchantID:   <input type="text" name="merchantId" ref="merchantID" placeholder="MerchantID" />
                </label>

                <input type="submit" value="Submit" />
                
                { FetchData.displayTable(paymentList) }
          </form>

        );
    }

    static displayTable(paymentList) {
        if (paymentList == null) {
            return <div></div>
        } else {
            return <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>CustomerID</th>
                        <th>Date</th>
                        <th>Amount</th>
                        <th>Fees</th>
                        <th>Currency</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {paymentList.map(payment =>
                        <tr key={payment.id}>
                            <td>{payment.customerId}</td>
                            <td>{payment.dateCreated}</td>
                            <td>{payment.amount}</td>
                            <td>{payment.fee}</td>
                            <td>{payment.currency}</td>
                            <td>{payment.status}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        }
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderPaymentsTable(this.state.paymentsResult);

        return (
            <div>
                 <h1 id="tabelLabel">Payments List</h1> 
                {contents}
            </div>
        );
    }

    async fetchPaymentsForMerchant(merchantId) {
        const response = await fetch('/api/PaymentItems/merchantid/' + merchantId);
        const data = await response.json();
        console.log(data);
        if (response.status == 200) {
            this.setState({ paymentsResult: data});
        }

        this.setState({ loading: false });
    }
}
